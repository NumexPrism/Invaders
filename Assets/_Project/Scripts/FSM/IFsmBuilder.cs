namespace FSM
{
  public interface IFsmBuilder<TState, TSignal>
  {
    IFsmBuilder<TState, TSignal> AddState(TState id);
    IFsmBuilder<TState, TSignal> AddTransition(TState source, TSignal signal, TState target);
    Fsm<TState,TSignal> StartInState(TState entry);
  }
}