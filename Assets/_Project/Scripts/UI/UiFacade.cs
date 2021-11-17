using System;
using System.Collections.Generic;
using UI.Views.Leaderboard;
using UI.Views.MainMenu;
using UnityEngine;
using Zenject;

namespace UI
{
  class UiFacade : MonoBehaviour, IUiFacade
  {
    private FSM fsm;

    [Inject] private LoadingView _loadingView;
    [Inject] private MainMenuView _mainMenu;
    [Inject] private GameView _gameView;
    [Inject] private GameEndView _gameEndView;
    [Inject] private LeaderBoardView _leaderBoardView;

    IEnumerable<IUiView> AllViews
    {
      get
      {
        yield return _loadingView;
        yield return _mainMenu;
        yield return _gameView;
        yield return _gameEndView;
        yield return _leaderBoardView;
      }
    }

    public void Start()
    {
      fsm = FSM.Build()
        .AddState(UiFsmStateId.Loading,     new UiState())
        .AddState(UiFsmStateId.MainMenu,    new UiState())
        .AddState(UiFsmStateId.Game,        new UiState())
        .AddState(UiFsmStateId.GameOver,    new UiState())
        .AddState(UiFsmStateId.Leaderboard, new UiState())

        .AddTransition(UiFsmStateId.Loading,     UiFsmSignalId.Next,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.MainMenu,    UiFsmSignalId.Next,        UiFsmStateId.Game)
        .AddTransition(UiFsmStateId.MainMenu,    UiFsmSignalId.LeaderBoard, UiFsmStateId.Leaderboard)
        .AddTransition(UiFsmStateId.Leaderboard, UiFsmSignalId.Back,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.Game,        UiFsmSignalId.Back,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.Game,        UiFsmSignalId.GameOver,    UiFsmStateId.GameOver)
        .AddTransition(UiFsmStateId.GameOver,    UiFsmSignalId.Next,        UiFsmStateId.MainMenu)

        .SetEntryState(UiFsmStateId.Loading);
      fsm.Start();

      var currentActiveView = ViewById(fsm.CurrentStateId);
      foreach (var view in AllViews)
      {
        if (currentActiveView == view)
        {
          view.Show();
        }
        else
        {
          view.Hide();
        }
      }

      fsm.StateChanged += ChangeUiPanel;
    }

    private void ChangeUiPanel(string previous, string current)
    {
      var previousView = ViewById(previous);
      var currentView = ViewById(current);
      previousView.Hide();
      currentView.Show();
    }

    private IUiView ViewById(string id)
    {
      switch (id)
      {
        case UiFsmStateId.Game: return _gameView;
        case UiFsmStateId.GameOver: return _gameEndView;
        case UiFsmStateId.Leaderboard: return _leaderBoardView;
        case UiFsmStateId.Loading: return _loadingView;
        case UiFsmStateId.MainMenu: return _mainMenu;
        default: throw new ArgumentException($"view {id} is not coinfigured");
      }
    }

    public bool ShowNextView()
    {
      return fsm.ProcessSignal(UiFsmSignalId.Next);
    }

    public bool ShowPreviousView()
    {
      return fsm.ProcessSignal(UiFsmSignalId.Back);
    }

    public bool ShowLeaderBoard()
    {
      return fsm.ProcessSignal(UiFsmSignalId.LeaderBoard);
    }

    public bool ShowGameOverView()
    {
      return fsm.ProcessSignal(UiFsmSignalId.GameOver);
    }
  }
}