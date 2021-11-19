using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LeaderBoard
{
  internal interface ILeaderBoardAdapter
  {
    UniTask<IReadOnlyCollection<LeaderBoardEntry>> LoadScoresOrdered();
    UniTask PublishScore(int score);
  }
}