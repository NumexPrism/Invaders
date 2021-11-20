using Utils;

namespace Mechanics.GameRules
{
  class GameSessionProxy : IGameSession
  {
    public IObservable<int> PlayerLives { get; private set; } = new Observable<int>(0);
    public IObservable<int> PlayerScore { get; private set; } = new Observable<int>(0);
    public IObservable<int> WaveCount { get; private set; } = new Observable<int>(0);

    public void CopyFrom(IGameSession source)
    {
      PlayerLives = new Observable<int>(source.PlayerLives.Value);
      PlayerScore = new Observable<int>(source.PlayerLives.Value);
      WaveCount = new Observable<int>(source.PlayerLives.Value);
    }
  }
}