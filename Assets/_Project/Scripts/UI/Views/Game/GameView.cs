using DependencyInjection.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
  [RequireComponent(typeof(GameViewMonoInstaller))]
  internal class GameView: BaseUIView
  {
    //rule of three at work: _backButton is just copied from the LeaderBoard View. If I have a third occurrence, I will introduce a base class, or move to composite
    [Inject] private Button _backButton;

    private void OnEnable()
    {
      _backButton.onClick.AddListener(BackButtonClicked);
    }

    private void OnDisable()
    {
      _backButton.onClick.RemoveListener(BackButtonClicked);
    }

    private void BackButtonClicked()
    {
      UiFacade.ShowPreviousView();
    }
  }
}