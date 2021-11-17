using UI.Views.MainMenu;
using UnityEngine.UI;

namespace DependencyInjection.UI
{
  class MainMenuViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    public Button StartButton;
    public Button HighScoreButton;
    public Button ExitButton;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(ExitButton).WithId(ButtonId.Exit);
      Container.BindInstance(HighScoreButton).WithId(ButtonId.HighScore);
      Container.BindInstance(StartButton).WithId(ButtonId.Start);
    }
  }
}