using System;
using LeaderBoard;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Widgets
{
  public class ScoreWidget : MonoBehaviour, IPoolable<LeaderBoardEntry, IMemoryPool>, IDisposable
  {
    internal class Pool : MonoMemoryPool<LeaderBoardEntry, ScoreWidget> { }
    public class Factory : PlaceholderFactory<LeaderBoardEntry, ScoreWidget> { }

    //Look how easy it's to setup dependencies in a widget
    //such a bliss =)
    //setting up with Zenject manually is just achieving the same with extra work
    //setting up automatically is bad for performance, because it breaks down to GetComponent<>

    [SerializeField] private TextMeshProUGUI nameWidget;
    [SerializeField] private TextMeshProUGUI score;
    private IMemoryPool _pool;

    public void OnDespawned()
    {
      _pool = null;
    }

    public void OnSpawned(LeaderBoardEntry entry, IMemoryPool pool)
    {
      if (entry == null)
      {
        Dispose();
        return;
      }
      //ToDo: inject them fields
      nameWidget.text = entry.name;
      score.text = Mathf.Clamp(entry.score, 0, 999999).ToString("D6");
      _pool = pool;
    }

    public void Dispose()
    {
      _pool.Despawn(this);
    }
  }
}
