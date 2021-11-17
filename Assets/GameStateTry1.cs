//using Input;
//using UnityEngine;
//using Zenject;
//
//internal class GameFSMSignal
//{
//  public const string Start = "AnyKey";
//  public const string Pause = "Continue";
//  public const string Death = "Death";
//}
//
//internal class GameStateId
//{
//  public const string Playing = "Playing";  
//  public const string Paused = "Paused";  
//}
//
//public class GameState : MonoBehaviour
//{
//  [Inject] private PlayingState _playingState;
//  [Inject] private WaitingState _waitingState;
//
//  private FSM _fsm;
//
//  // Start is called before the first frame update
//  void Start()
//  {
//    _fsm = FSM.Build()
//      .AddState(GameStateId.Playing, _playingState)
//      .AddState(GameStateId.Paused, _waitingState)
//      .AddTransition(GameStateId.Playing, GameFSMSignal.Start, GameStateId.Playing)
//      .AddTransition(GameStateId.Paused, GameFSMSignal.Pause, GameStateId.Paused)
//      .SetEntryState(GameStateId.Paused);
//    _fsm.Start();
//  }
//}
//
//public class PlayingState: BaseFsmState
//{
//}
//
//public class WaitingState : BaseFsmState
//{
//  [Inject] private IGameInput _input;
//
//  public override void OnEnter()
//  {
//    _input.Shoot += SwitchToPlaying;
//  }
//
//  private void SwitchToPlaying()
//  {
//    Fsm.ProcessSignal(GameFSMSignal.Start);
//  }
//
//  public override void OnExit()
//  {
//    _input.Shoot -= SwitchToPlaying;
//  }
//}