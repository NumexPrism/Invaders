public abstract class BaseFsmState : IFsmState, IFsmStateConfigurator
{
  public void LinkFsmState(FSM fsm)
  {
    FSM = fsm;
  }

  public FSM FSM { get; private set; }

  public virtual void OnEnter()
  {
  }

  public virtual void OnExit()
  {
  }
}