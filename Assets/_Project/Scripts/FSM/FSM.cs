using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
  protected FSM() {}

  private Dictionary<string, IFsmState>_states
    = new Dictionary<string, IFsmState>();

  private Dictionary<string, Dictionary<string, string>> _transitions =
    new Dictionary<string, Dictionary<string, string>>();

  private string _currentStateId;

  public static IFsmBuilder Build()
  {
    return new Builder();
  }

  private class Builder : IFsmBuilder
  {
    private FSM _sm;

    public Builder()
    {
      _sm = new FSM();
    }

    public IFsmBuilder AddState<TFsmState>(string id, TFsmState state) where TFsmState : IFsmState, IFsmStateConfigurator
    {
      _sm._states[id] = state;
      state.LinkFsmState(_sm); 
      return this;
    }

    public IFsmBuilder AddTransition(string source, string signal, string target)
    {
      if (!_sm._states.ContainsKey(source))
        throw new ArgumentException($"source is {source} but has no state assigned. please use AddState({source}, ...)");

      if (!_sm._transitions.ContainsKey(source))
      {
        _sm._transitions[source] = new Dictionary<string, string>();
      }

      _sm._transitions[source][signal] = target;

      return this;
    }

    public FSM SetEntryState(string entry)
    {
      _sm._currentStateId = entry;
      return _sm;
    }
  }

  public void Start()
  {
    var state = _states[_currentStateId];
    state.OnEnter();
  }

  private readonly Queue<string> signals = new Queue<string>(1);

  public void SendSignal(string signal)
  {
    if (signals.Count != 0)
    {
      Debug.LogError("recursive state change is not supported");
      return;
    }

    signals.Enqueue(signal);

    if (!_transitions.ContainsKey(_currentStateId))
    {
      Debug.Log($"State {_currentStateId} has no rule for signal {signal}. Nothing happens");
      signals.Dequeue();
      return;
    }

    try
    {
      var nextStateId = _transitions[_currentStateId][signal];

      StateChanging?.Invoke(_currentStateId, nextStateId);

      try
      {
        var state = _states[_currentStateId];
        state.OnExit();
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }

      _currentStateId = nextStateId;
      var nextState = _states[nextStateId];
      nextState.OnEnter();

      StateChanged?.Invoke(_currentStateId, nextStateId);
    }
    finally
    {
      signals.Dequeue();
    }
  }

  /// <summary>
  /// Called before changes are applied
  /// </summary>
  public event Action<string, string> StateChanging;

  /// <summary>
  /// Called after changes are applied
  /// </summary>
  public event Action<string,string> StateChanged;
}