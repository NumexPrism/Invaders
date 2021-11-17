using UnityEngine;
using Zenject;

namespace UI
{
  [RequireComponent(typeof(CanvasGroup))]
  [RequireComponent(typeof(GameObjectContext))]
  internal class BaseUIView :MonoBehaviour, IUiView
  {
    [Inject] protected CanvasGroup CanvasGroup;
    [Inject] protected IUiFacade UiFacade;

    protected virtual void Start()
    {
    }

    public virtual void Hide()
    {
      //ToDo:disappearAnimation
      gameObject.SetActive(false);
    }

    public virtual void Show()
    {
      gameObject.SetActive(true);
      //ToDo: Appear animation.
    }
  }
}