using Zenject;

namespace Mechanics.Projectiles
{
  class ObjectPoolFactory<T> : IFactory<T>
  {
    public T Create()
    {
      throw new System.NotImplementedException();
    }
  }
}