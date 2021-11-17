﻿using UI;
using UI.Views.Leaderboard;
using UI.Views.MainMenu;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DependencyInjection
{
  class UiInstaller : MonoInstaller<UiInstaller>
  {
    [Header("UiView prefab addresses")]
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

    void InstallUiViewFromPrefab<TUiView>(Object uiViewPrefab) where TUiView: IUiView 
         => Container.Bind<TUiView>()
        .FromComponentInNewPrefab(uiViewPrefab)
        .AsSingle()
        .CopyIntoDirectSubContainers();
  }
}

