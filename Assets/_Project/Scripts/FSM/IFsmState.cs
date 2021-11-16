public interface IFsmState
{
  FSM FSM { get; }
  void OnEnter();
  void OnExit();
}