using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Mechanics
{
  internal class Metronome: IMetronome
  {
    private class TickFilter: IMetronome
    {
      public event Action Tick;

      public void Invoke()
      {
        Tick?.Invoke();;
      }
    }

    public event Action Tick;

    private TimeSpan _interval = TimeSpan.FromSeconds(0.5f);
    public float IntervalSeconds => (float) _interval.TotalSeconds;

    private int _tickNumber;

    private Dictionary<int, TickFilter> _filters = new Dictionary<int, TickFilter>();
    private bool _isActive = false;

    public IMetronome OnEvery(int n)
    {
      if (!_filters.ContainsKey(n))
      {
        _filters.Add(n, new TickFilter());
      }
      return _filters[n];
    }

    public float Bpm /*120*/
    {
      get => (float)(60.0 / _interval.TotalSeconds);
      set => _interval = TimeSpan.FromSeconds(60.0 / value);
    }

    public async void Start(float delaySeconds, bool resetTicks = true)
    {
      if(resetTicks)
        _tickNumber = 0;
      _isActive = true;

      await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds));
      while (_isActive)
      {
        _tickNumber++;
        Tick?.Invoke();
        foreach (var tickOnNthBeat in _filters)
        {
          if (_tickNumber % tickOnNthBeat.Key == 0)
          {
            tickOnNthBeat.Value.Invoke();
          }
        }
        await UniTask.Delay(_interval, DelayType.DeltaTime, PlayerLoopTiming.PreUpdate);
      }
    }

    public void Stop()
    {
      _isActive = false;
    }
  }
}