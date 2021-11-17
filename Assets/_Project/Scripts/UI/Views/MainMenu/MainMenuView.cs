using System;
using DependencyInjection.UI;
using UI.Views.MainMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
  [RequireComponent(typeof(MainMenuViewMonoInstaller))]
  internal class MainMenuView: BaseUIView
  {
    [Inject(Id = ButtonId.Start)] private Button _startButton;
    [Inject(Id = ButtonId.HighScore)] private Button _leaderBoardButton;
    [Inject(Id = ButtonId.Exit)] private Button _exitButton;

    private void OnEnable()
    {
      _startButton.onClick.AddListener(StartButtonClicked);
      _leaderBoardButton.onClick.AddListener(LeaderBoardButtonClicked);
      _exitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void OnDisable()
    {
      _startButton.onClick.RemoveListener(StartButtonClicked);
      _leaderBoardButton.onClick.RemoveListener(LeaderBoardButtonClicked);
      _exitButton.onClick.RemoveListener(ExitButtonClicked);
    }

    private void StartButtonClicked()
    {
      UiFacade.ShowNextView();
    }

    private void LeaderBoardButtonClicked()
    {
      UiFacade.ShowLeaderBoard();
    }

    private void ExitButtonClicked()
    {
      Application.Quit();
    }
  }
}