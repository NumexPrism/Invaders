using System;

namespace Utils
{
  internal interface IObservable<T>
  {
    void Observe(Action<T> onChanged);
    void StopObserving(Action<T> onChanged);
  }
}