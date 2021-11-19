using System;
using JetBrains.Annotations;

namespace Mechanics
{
  class Observable<T>: IObservable<T> 
  {
    private T _value;
    public event Action<T> Changed;

    public static implicit operator T(Observable<T> d) => d._value;

    public Observable(T value)
    {
      this._value = value;
      Changed = null;
    }

    public void Set(T value)
    {
      _value = value;
      Changed?.Invoke(_value);
    }

    public void Observe([NotNull] Action<T> onChanged)
    {
      if(onChanged == null)
        throw new ArgumentNullException(nameof(onChanged));
      Changed += onChanged;
      onChanged(_value);
    }

    public void StopObserving(Action<T> onChanged)
    {
      Changed -= onChanged;
    }
  }
}