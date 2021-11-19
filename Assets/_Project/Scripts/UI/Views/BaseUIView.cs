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

    public virtual void Hide()
    {
      //ToDo:disappearAnimation
      gameObject.SetActive(false);
      enabled = false;
    }

    public virtual void Show()
    {
      gameObject.SetActive(true);
      enabled = true;
      //ToDo: Appear animation.
    }
  }
}