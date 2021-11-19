using UI.Views.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Installers.Project.UI.UiPanel
{
  class MainMenuViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button startButton;
    [SerializeField] private Button leaderBoardButton;
    [SerializeField] private Button exitButton;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(exitButton).WithId(ButtonId.Exit);
      Container.BindInstance(leaderBoardButton).WithId(ButtonId.HighScore);
      Container.BindInstance(startButton).WithId(ButtonId.Start);
    }
  }
}