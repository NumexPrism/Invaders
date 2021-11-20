using Utils;

namespace Mechanics.GameRules
{
  internal interface IGameSession
  {
    IObservable<int> PlayerLives { get; }
    IObservable<int> PlayerScore { get; }
    IObservable<int> WaveCount { get; }
  }
}