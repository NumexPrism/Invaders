namespace FSM
{
  public interface IFsmBuilder
  {
    IFsmBuilder AddState<TFsmState>(string id, TFsmState state) where TFsmState : IFsmState, IFsmStateConfigurator;
    IFsmBuilder AddTransition(string source, string signal, string target);
    FSM SetEntryState(string entry);
  }
}