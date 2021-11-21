using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
  public class Fsm<TStateId, TSignal>
  {
    //I'm not particularly a fan of FSMs, because the topic is really vague, and everyone means something different when says "StateMachine"
    //FSM from the algorithmic point of view is just a graph, where nodes are states and links are "transitions"
    //each transition is labeled with signal, FSM receives a signal, and if current active state has a link with that label
    //it just switches the active state to where the link points to. that's it. Not really useful on it's own
    //
    //I've seen quite a lot of variations - with async reversible transitions, with parametrized transitions, with nested state machines, with tickable states, with blackboard 
    
    //state machines usually excel at situations where there is a set of signals, but the system should process each signal differently, depending on it's state
    //a good example is animation. There is a signal "jump" and depending on current state "idle" or "run" the character will play different anims.
    
    protected Fsm() {}

    private readonly HashSet<TStateId> _states = new HashSet<TStateId>();
 
    private readonly Dictionary<TStateId, Dictionary<TSignal, TStateId>> _transitions =
      new Dictionary<TStateId, Dictionary<TSignal, TStateId>>();

    public TStateId CurrentStateId { get; private set; }

    public static IFsmBuilder<TStateId, TSignal> Build()
    {
      return new Builder();
    }

    //on the topic of nested classes. They have their pros and cons, and quite controversial to use.
    //in some cases, with factories and builders and classes who should be hidden from the "outer world" they are OK.
    //it's functionally similar to c++ friend classes - a way to allow one class to access privates of another.
    private class Builder : IFsmBuilder<TStateId, TSignal>
    {
      private readonly Fsm<TStateId, TSignal> _sm;

      public Builder()
      {
        _sm = new Fsm<TStateId, TSignal> ();
      }

      public IFsmBuilder<TStateId, TSignal> AddState(TStateId stateId)
      {
        _sm._states.Add(stateId);
        return this;
      }

      public IFsmBuilder<TStateId, TSignal> AddTransition(TStateId source, TSignal signal, TStateId target)
      {
        if (!_sm._states.Contains(source))
          throw new ArgumentException($"source is {source} but has no state assigned. please use AddState({source}, ...)");

        if (!_sm._transitions.ContainsKey(source))
        {
          _sm._transitions[source] = new Dictionary<TSignal, TStateId>();
        }

        _sm._transitions[source][signal] = target;

        return this;
      }

      public Fsm<TStateId, TSignal>  StartInState(TStateId entry)
      {
        _sm.CurrentStateId = entry;
        return _sm;
      }
    }

    private readonly Queue<TSignal> _signals = new Queue<TSignal>(1);

    /// <summary>
    /// Called before changes are applied
    /// </summary>
    public event Action<TStateId, TStateId> StateChanging;

    /// <summary>
    /// Called after changes are applied
    /// </summary>
    public event Action<TStateId, TStateId> StateChanged;

    public bool ProcessSignal(TSignal signal)
    {
      if (_signals.Count != 0)
      {
        Debug.LogError("recursive state change is not supported");
        return false;
      }

      _signals.Enqueue(signal);

      if (!_transitions.ContainsKey(CurrentStateId))
      {
        Debug.Log($"State {CurrentStateId} has no rule for signal {signal}. Nothing happens");
        _signals.Dequeue();
        return false;
      }

      try
      {
        var previousStateId = CurrentStateId;
        var nextStateId = _transitions[CurrentStateId][signal];

        StateChanging?.Invoke(CurrentStateId, nextStateId);
        CurrentStateId = nextStateId;
        StateChanged?.Invoke(previousStateId, CurrentStateId);
      }
      finally
      {
        _signals.Dequeue();
      }
      return true;
    }

    //For debugging purposes
#if UNITY_EDITOR
    public void ForceChangeState(TStateId newStateId)
    {
      //one cool way to approach it would be to find the shortest path in the state graph
      //and traverse the way, so it would be valid from the SM perspective.

      //Since it's a test task, i will just hop to the requested state, and mark the method as editor only
      var previousStateId = CurrentStateId;
      StateChanging?.Invoke(CurrentStateId, newStateId);
      CurrentStateId = newStateId;
      StateChanged?.Invoke(previousStateId, newStateId);
    }
#endif

  }
}