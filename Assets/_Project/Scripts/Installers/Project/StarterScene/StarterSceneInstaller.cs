using Loading;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers.Project.StarterScene
{
  class StarterSceneInstaller: MonoInstaller<UiInstaller>
  {
    [FormerlySerializedAs("Starter")] [SerializeField] private Starter starter; 
    [SerializeField] private UiFacade uiFacade; 

    public override void InstallBindings()
    {
      Container.BindInstance(starter);
      
      //push from scene context to project context. see gameSceneInstaller for more comments
      foreach (var parentContainer in Container.ParentContainers)
      {
        //I've put loading UI in the scene, to avoid screen flicks. Unity will load it fully before rendering any frames.
        parentContainer.BindInterfacesAndSelfTo<UiFacade>().FromInstance(uiFacade);
      }
    }
  }
}
