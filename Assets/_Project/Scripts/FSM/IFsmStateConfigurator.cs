namespace FSM
{
  public interface IFsmStateConfigurator
  {
    void SetId(string id);
    void LinkFsmState(FSM state);
  }
}