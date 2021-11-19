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
      Debug.Log($"{this.GetType()} HIDEDN (GO ={gameObject.name})");
      gameObject.SetActive(false);
    }

    public virtual void Show()
    {
      Debug.Log($"{this.GetType()} SHOWN (GO ={gameObject.name})");
      gameObject.SetActive(true);
      //ToDo: Appear animation.
    }
  }
}