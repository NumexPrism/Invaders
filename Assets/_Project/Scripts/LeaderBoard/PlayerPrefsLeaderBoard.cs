using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LeaderBoard
{
  internal class PlayerPrefsLeaderBoard : ILeaderBoardAdapter
  {
    [Serializable]
    class EntryList
    {
      //JsonUtility can't serialize lists. object wrapper does the trick
      public List<LeaderBoardEntry> entries = new List<LeaderBoardEntry>();
    }

    [Inject] private IPlayerProfileAdapter profile;

    private const string LeaderBoardKey = "Leaderboard";

    public async UniTask<IReadOnlyCollection<LeaderBoardEntry>> LoadScoresOrdered()
    {
      await UniTask.Delay(Random.Range(500, 1500));
      return new ReadOnlyCollection<LeaderBoardEntry>(GetValues.OrderBy(r => r.score).ToList());
    }

    public IEnumerable<LeaderBoardEntry> GetValues
    {
      get
      {
        var leaderBoardJson = PlayerPrefs.GetString(LeaderBoardKey, string.Empty);
        List<LeaderBoardEntry> list = JsonUtility.FromJson<EntryList>(leaderBoardJson)?.entries ?? new List<LeaderBoardEntry>();
        return list;
      }
    }

    public async UniTask PublishScore(int score)
    {
      var name = await profile.PlayerName;

      //I can't even run playerprefs asyncronously, because they only work on main thread.
      //
      //I could have use some brainpower to serialize to a file in application folder, but I don't have any brainpower left.
      //a simple local file would work asyncronously just fine. It would just take an hour or so to do properly, against 5 min with player prefs. This task is already way too long.
      //and in live operation it would require some versioning, backward compatibility, error handling. 

      var leaderBoardJson = PlayerPrefs.GetString(LeaderBoardKey, string.Empty);
      List<LeaderBoardEntry> list = JsonUtility.FromJson<EntryList>(leaderBoardJson)?.entries ?? new List<LeaderBoardEntry>();

      list.Add(new LeaderBoardEntry() { name = name, score = score });

      //and even with simple solutions I'm aware to do it good. If I'd just appended the prefs, they would have gone huge really fast. Instead I trim the leaderboard
      var newLeaderBoard = list.OrderByDescending(e => e.score).Take(6).ToList();
      var leaderBoardSerialized = JsonUtility.ToJson(new EntryList(){entries = newLeaderBoard});
      PlayerPrefs.SetString(LeaderBoardKey,leaderBoardSerialized);
      PlayerPrefs.Save();
    }
  }
}