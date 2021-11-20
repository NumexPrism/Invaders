using System;

namespace Utils
{
  internal interface IObservable<out T>
  {
    T Value { get; }
    void Observe(Action<T> onChanged);
    void StopObserving(Action<T> onChanged);
  }
}