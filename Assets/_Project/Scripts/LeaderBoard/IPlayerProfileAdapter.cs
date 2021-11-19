using Cysharp.Threading.Tasks;

namespace LeaderBoard
{
  interface IPlayerProfileAdapter
  {
    UniTask<string> PlayerName { get; }
  }

  class StubPlayerProfile : IPlayerProfileAdapter
  {
    public UniTask<string> PlayerName => UniTask.FromResult("PLAYER 1");
  }
}