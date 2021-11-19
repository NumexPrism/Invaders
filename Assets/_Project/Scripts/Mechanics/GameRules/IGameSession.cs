using Utils;

namespace Mechanics.GameRules
{
  internal interface IGameSession
  {
    IObservable<int> PlayerLives { get; }
    IObservable<int> PlayerScore { get; }
    IObservable<int> WaveCount { get; }
  }

  class NullGameSession : IGameSession
  {
    public IObservable<int> PlayerLives { get; } = new Observable<int>(3);
    public IObservable<int> PlayerScore { get; } = new Observable<int>(0);
    public IObservable<int> WaveCount { get; } = new Observable<int>(0);
  }
}