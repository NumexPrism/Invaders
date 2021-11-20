using System;
using JetBrains.Annotations;

namespace Utils
{
  class Observable<T>: IObservable<T> 
  {
    public T Value { get; private set; }

    public event Action<T> Changed;

    public static implicit operator T(Observable<T> d) => d.Value;

    public Observable(T value)
    {
      this.Value = value;
      Changed = null;
    }

    public void Set(T value)
    {
      Value = value;
      Changed?.Invoke(Value);
    }

    public void Observe([NotNull] Action<T> onChanged)
    {
      if(onChanged == null)
        throw new ArgumentNullException(nameof(onChanged));
      Changed += onChanged;
      onChanged(Value);
    }

    public void StopObserving(Action<T> onChanged)
    {
      Changed -= onChanged;
    }
  }
}