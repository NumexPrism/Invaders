using System;
using Mechanics.GameRules;
using Zenject;

namespace Mechanics.Player
{
  class Blinker
  {
    private readonly Metronome _metronome;
    private Action _on;
    private Action _off;
    private bool isOff = true;

    [Inject] public Blinker(Metronome metronome)
    {
      _metronome = metronome;
      _metronome.Tick += OnTick;
    }

    private void OnTick()
    {
      if (isOff)
      {
        _on();
        isOff = false;
      }
      else
      {
        _off();
        isOff = true;
      }
    }

    public void Start(float interval, Action On, Action Off)
    {
      _on = On;
      _off = Off;
      _metronome.IntervalSeconds = interval;
      _metronome.Start(0);
    }

    public void Stop()
    {
      _metronome.Stop();
      if (!isOff)
      {
        isOff = true;
        _off();
      }
    }
  }
}