public interface IFsmState
{
  string Id { get; }
  FSM Fsm { get; }

  void OnEnter();
  void OnExit();
}