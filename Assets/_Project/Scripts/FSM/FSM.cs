using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
  public class FSM
  {
    protected FSM() {}

    private Dictionary<string, IFsmState>_states
      = new Dictionary<string, IFsmState>();

    private Dictionary<string, Dictionary<string, string>> _transitions =
      new Dictionary<string, Dictionary<string, string>>();

    public string CurrentStateId { get; private set; }

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
        state.SetId(id);
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
        _sm.CurrentStateId = entry;
        return _sm;
      }
    }

    public void Start()
    {
      var state = _states[CurrentStateId];
      state.OnEnter();
    }

    private readonly Queue<string> signals = new Queue<string>(1);

    public bool ProcessSignal(string signal)
    {
      if (signals.Count != 0)
      {
        Debug.LogError("recursive state change is not supported");
        return false;
      }

      signals.Enqueue(signal);

      if (!_transitions.ContainsKey(CurrentStateId))
      {
        Debug.Log($"State {CurrentStateId} has no rule for signal {signal}. Nothing happens");
        signals.Dequeue();
        return false;
      }

      try
      {
        var previousStateId = CurrentStateId;
        var nextStateId = _transitions[CurrentStateId][signal];

        StateChanging?.Invoke(CurrentStateId, nextStateId);

        try
        {
          var state = _states[CurrentStateId];
          state.OnExit();
        }
        catch (Exception e)
        {
          Debug.LogException(e);
        }

        CurrentStateId = nextStateId;
        var nextState = _states[nextStateId];
        nextState.OnEnter();

        StateChanged?.Invoke(previousStateId, nextStateId);
      }
      catch (Exception e)
      {
        throw;
      }
      finally
      {
        signals.Dequeue();
      }
      return true;
    }

    //For debugging purposes
#if UNITY_EDITOR
    public void ForceChangeState(string id)
    {
      //one cool way to approach it would be to find the shortest path in the state graph
      //and traverse the way, so it would be valid from the SM perspective.

      //Since it's a test task, i'will just hop to the requested state, and mark the method as editor only

      var previousStateId = CurrentStateId;
      StateChanging?.Invoke(CurrentStateId, id);
      CurrentStateId = id;
      StateChanged?.Invoke(previousStateId, id);
    }
#endif


    /// <summary>
    /// Called before changes are applied
    /// </summary>
    public event Action<string, string> StateChanging;

    /// <summary>
    /// Called after changes are applied
    /// </summary>
    public event Action<string,string> StateChanged;
  }
}