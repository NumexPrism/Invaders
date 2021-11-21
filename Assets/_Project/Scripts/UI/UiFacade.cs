using System;
using System.Collections.Generic;
using Input;
using UI.Views.Game;
using UI.Views.GameEnd;
using UI.Views.LeaderBoard;
using UI.Views.Loading;
using UI.Views.MainMenu;
using UnityEngine;
using Zenject;

namespace UI
{
  class UiFacade : MonoBehaviour, IUiFacade, IUiDebug, IGameInput
  {
    private FSM.Fsm<string, string> _fsm;

    [Inject] private DiContainer _container;

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
      RegisterInUiComposite();

      _gameView.ShootCallback = () => { Shoot?.Invoke();};
      _gameView.MoveStartCallback = dir => { MoveStarted?.Invoke(dir); };
      _gameView.MoveEndCallback = () => { MoveStopped?.Invoke();};

      _fsm = FSM.Fsm<string,string>.Build()
        .AddState(UiFsmStateId.Loading)
        .AddState(UiFsmStateId.MainMenu)
        .AddState(UiFsmStateId.Game)
        .AddState(UiFsmStateId.GameOver)
        .AddState(UiFsmStateId.Leaderboard)

        .AddTransition(UiFsmStateId.Loading,     UiFsmSignalId.Next,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.MainMenu,    UiFsmSignalId.Next,        UiFsmStateId.Game)
        .AddTransition(UiFsmStateId.MainMenu,    UiFsmSignalId.LeaderBoard, UiFsmStateId.Leaderboard)
        .AddTransition(UiFsmStateId.Leaderboard, UiFsmSignalId.Back,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.Game,        UiFsmSignalId.Back,        UiFsmStateId.MainMenu)
        .AddTransition(UiFsmStateId.Game,        UiFsmSignalId.Next,        UiFsmStateId.GameOver)
        .AddTransition(UiFsmStateId.GameOver,    UiFsmSignalId.Next,        UiFsmStateId.MainMenu)

        .StartInState(UiFsmStateId.Loading);

      var currentActiveView = ViewById(_fsm.CurrentStateId);

      //all views are hidden by default
      foreach (var view in AllViews)
      {
        if (currentActiveView == view)
        {
          view.Show();
        }
      }

      _fsm.StateChanged += OnUiFsmChanged;
    }

    private void OnDestroy()
    {
      _container.Resolve<GameInputComposite>().Remove(this);
    }

    private void RegisterInUiComposite()
    {
      _container.Resolve<GameInputComposite>().Add(this);
    }

    private void OnUiFsmChanged(string previous, string current)
    {
      ChangeUiPanel(previous,current);
      if (previous == UiFsmStateId.Game && current == UiFsmStateId.MainMenu)
      {
        GameStopped?.Invoke();
      }
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
      return _fsm.ProcessSignal(UiFsmSignalId.Next);
    }

    public bool ShowPreviousView()
    {
      return _fsm.ProcessSignal(UiFsmSignalId.Back);
    }

    public bool ShowLeaderBoard()
    {
      return _fsm.ProcessSignal(UiFsmSignalId.LeaderBoard);
    }

    public event Action GameStopped;

#if UNITY_EDITOR
    public void ForceSwitchToGameUi()
    {
      _fsm.ForceChangeState(UiFsmStateId.Game);
    }
#endif

    public event Action<float> MoveStarted;
    public event Action MoveStopped;
    public event Action Shoot;
  }
}