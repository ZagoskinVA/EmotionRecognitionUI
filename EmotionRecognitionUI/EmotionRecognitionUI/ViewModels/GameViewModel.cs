﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using EmotionRecognitionUI.Events;
using EmotionRecognitionUI.Models;
using EmotionRecognitionUI.Services;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace EmotionRecognitionUI.ViewModels;

public class GameViewModel: ViewModelBase
{
    private readonly IYandexDriveService _yandexDriveService;
    private readonly IEventAggregator _eventAggregator;
    private List<ImageModel> _images;
    private Bitmap? _image;
    private int _imageIndex = 0;
    private int _imageCount = 0;
    private string _scoring ="";
    private List<string> _answers;
    private readonly Random _random = new Random();
    public ICommand ChooseLabelCommand { get; }

    public Bitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value); 
    }
    
    public int ImageIndex
    {
        get => _imageIndex;
        set
        {
            _imageIndex = value;
            UpdateScoring();
        }
    }

    public int ImageCount
    {
        get => _imageCount;
        set
        {
            _imageCount = value;
            UpdateScoring();
        }
    }

    public string Scoring
    {
        get => _scoring;
        set => this.RaiseAndSetIfChanged(ref _scoring, value);

    }
    
    private string _textBoxForeground = "White";
    public string TextBoxForeground
    {
        get => _textBoxForeground;
        set => this.RaiseAndSetIfChanged(ref _textBoxForeground, value);
    }
    

    public GameViewModel(IYandexDriveService yandexDriveService, IEventAggregator eventAggregator)
    {
        _yandexDriveService = yandexDriveService;
        _eventAggregator = eventAggregator;
        Task.Run(async () => await LoadImagesAsync());
        ChooseLabelCommand = new DelegateCommand<string>(OnChooseLabel);
        _answers = new List<string>();
        Scoring = "";
        _eventAggregator.GetEvent<ChangeThemeModeEvent>().Subscribe((foreground) =>
        {
            TextBoxForeground = foreground;
        });

    }

    private async Task LoadImagesAsync()
    {
        var images = await _yandexDriveService.Get(2);
        
        _images = images.OrderBy(x => _random.Next()).ToList();
        Image = _images.First().Image;
        ImageIndex = 0;
        ImageCount = _images.Count;
    }

    public void MoveToNextImage()
    {
        if(ImageIndex == ImageCount - 1)
        {
            OpenScorePage();
            return;
        }
        ImageIndex += 1;
        Image = _images[ImageIndex].Image;
    }

    private void OpenScorePage()
    {
        _eventAggregator.GetEvent<ShowResultEvent>().Publish(new ScoringModel()
        {
            CorrectAnswers = _images.Select(x => x.Label).Zip(_answers).Count(x => (x.First.ToLower().Contains(x.Second.ToLower()))),
            AnswersCount = _answers.Count
        });
    }

    public void OnChooseLabel(string label)
    {
        _eventAggregator.GetEvent<OpenAnswerPageEvent>().Publish(new AnswerModel()
        {
            IsCorrect = _images[ImageIndex].Label.ToLower().Contains(label.ToLower()),
            CorrectAnswer = _images[ImageIndex].Label.ToLower(),
            Image = Image
        });
        _answers.Add(label);
    }
    
    private void UpdateScoring()
    {
        Scoring = $"Фото {ImageIndex + 1} из {ImageCount}";
    }
}