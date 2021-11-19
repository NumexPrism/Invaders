using Installers.Project.UI.UiPanel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.GameEnd
{
  [RequireComponent(typeof(GameEndViewMonoInstaller))]
  internal class GameEndView: BaseUIView
  {
    [Inject] private Button _nextButton;

    private void OnEnable()
    {
      _nextButton.onClick.AddListener(NextButtonClicked);
      //ToDo: Show the score for the run;
    }

    private void OnDisable()
    {
      _nextButton.onClick.RemoveListener(NextButtonClicked);
    }

    private void NextButtonClicked()
    {
      UiFacade.ShowNextView();
    }
  }
}