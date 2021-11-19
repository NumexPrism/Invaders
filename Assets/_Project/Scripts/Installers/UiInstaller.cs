using UI;
using UI.Views.Game;
using UI.Views.GameEnd;
using UI.Views.Leaderboard;
using UI.Views.Loading;
using UI.Views.MainMenu;
using UnityEngine;
using Zenject;

namespace Installers
{
  class UiInstaller : MonoInstaller<UiInstaller>
  {
    public LoadingView loadingUiView; 
    public MainMenuView mainMenu; 
    public GameView game; 
    public GameEndView gameEnd; 
    public LeaderBoardView leaderBoard; 

    public override void InstallBindings()
    {
      Container.BindInstance(loadingUiView)
        .AsSingle()
        .CopyIntoDirectSubContainers();

      InstallUiViewFromPrefab<MainMenuView>(mainMenu);
      InstallUiViewFromPrefab<GameView>(game);
      InstallUiViewFromPrefab<GameEndView>(gameEnd);
      InstallUiViewFromPrefab<LeaderBoardView>(leaderBoard);
    }

    void InstallUiViewFromPrefab<TUiView>(Object uiViewPrefab) where TUiView : IUiView
      => Container.Bind<TUiView>()
        .FromComponentInNewPrefab(uiViewPrefab)
        .AsSingle()
        .CopyIntoDirectSubContainers();
  }
}

