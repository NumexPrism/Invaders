using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI.Views
{
  [RequireComponent(typeof(CanvasGroup))]
  [RequireComponent(typeof(GameObjectContext))]
  internal class BaseUIView :MonoBehaviour, IUiView
  {
    [Inject] protected CanvasGroup CanvasGroup;
    [Inject] protected IUiFacade UiFacade;

    public void Awake()
    {
      gameObject.SetActive(false);
    }

    //overrides are basically a tool to break Liskov Principle. It's to easy to forget to call base.foo
    //templateMethod would be good here. But as you know, I'm short on time.
    public virtual UniTask Show()
    {
      gameObject.SetActive(true);
      return UniTask.CompletedTask;//can be changed to showing Animation
    }

    public virtual UniTask Hide()
    {
      return UniTask.CompletedTask//can be changed to hiding animation
        .ContinueWith(()=>gameObject.SetActive(false));
    }
  }
}