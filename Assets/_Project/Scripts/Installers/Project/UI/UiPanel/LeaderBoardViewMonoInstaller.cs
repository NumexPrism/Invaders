using LeaderBoard;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Installers.Project.UI.UiPanel
{
  class LeaderBoardViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;
    [SerializeField] private Image spinner;
    [SerializeField] private LayoutGroup widgetRoot;
    [SerializeField] private ScoreWidget widgetPrefab;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(backButton);
      Container.BindInstance(spinner);

      for (int i = 0; i < widgetRoot.transform.childCount; i++)
      {
        widgetRoot.transform.GetChild(i).gameObject.SetActive(false);
      }
      
      Container.BindFactory<LeaderBoardEntry, ScoreWidget, ScoreWidget.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(6)
          .FromComponentInNewPrefab(widgetPrefab)
          .UnderTransform(widgetRoot.transform));
    }
  }
}