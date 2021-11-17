public abstract class BaseFsmState : IFsmState, IFsmStateConfigurator
{
  public string Id { get; private set; }
  public void SetId(string id)
  {
    Id = id;
  }

  public FSM Fsm { get; private set; }
  public void LinkFsmState(FSM fsm)
  {
    this.Fsm = fsm;
  }

  public virtual void OnEnter()
  {
  }

  public virtual void OnExit()
  {
  }
}