 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
 using EmotionRecognitionUI.Models;
 using YandexDisk.Client;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace EmotionRecognitionUI.Services;


public interface IYandexDriveService
{
    Task<IEnumerable<ImageModel>> Get(int take = 14);
}

public class YandexDriveService: IYandexDriveService
{
    private readonly IDiskApi _diskApi;
    private readonly string _basePath;
    private string[] emotions = {"гнев", "отвращение", "печаль", "презрение", "радость", "страх", "удивление"};
    private Dictionary<string, string[]> _fileNames = new();
    private const string _token = "y0_AgAAAABID1eJAADLWwAAAADeCQgtmljootw6TvC6esBBaw7-tqNrhEM";


    public YandexDriveService()
    {
        _diskApi = new DiskHttpApi(_token);
        _basePath = "disk:/Emotions";
    }
    
    public async Task<IEnumerable<ImageModel>> Get(int take = 2)
    {
        var images = new List<ImageModel>(take);
        var fileNames = await GetFileNames();
        var random = new Random();
        var emotionFileNames = emotions.SelectMany(x =>
            fileNames[x].Select(y => new {value = y, order = random.Next()}).OrderBy(q => q.order).Take(take)
                .Select( (t) => new {label = x, emotion = t.value}).ToArray());
        foreach (var emotionFileName in emotionFileNames)
        {
            var path = $"{_basePath.Trim('/')}/{emotionFileName.label}/{emotionFileName.emotion}";
            var link = await _diskApi.Files.GetDownloadLinkAsync(path);
            var emotionFileStream = await _diskApi.Files.DownloadAsync(link);
            var image = ConvertStreamToImage(emotionFileStream);
            images.Add(new ImageModel(){ Image = image, Label = emotionFileName.label});
        }

        return images;
    }

    private Bitmap ConvertStreamToImage(Stream stream)
    {
        return new Bitmap(stream);
    }
    
    private async Task<Dictionary<string, string[]>> GetFileNames()
    {
        if (_fileNames.Count != 0) return _fileNames;

        foreach (var emotion in emotions)
        {
            var resources = await _diskApi.MetaInfo.GetInfoAsync(new ResourceRequest()
            {
                Path = $"{_basePath.Trim('/')}/{emotion}"
            });
            
            if(resources.HttpStatusCode == HttpStatusCode.OK)
                _fileNames.Add(emotion, resources.Embedded.Items.Where(x => x.Type == ResourceType.File).Select(x => x.Name).ToArray());
        }

        return _fileNames;
    }
}