using DependencyInjection.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Leaderboard
{
  [RequireComponent(typeof(LeaderBoardViewMonoInstaller))]
  internal class LeaderBoardView: BaseUIView
  {
    //Yagni principle at work: I can add ButtonId, if the view changes to have 2 or more buttons

    [Inject] private Button _backButton;

    private void OnEnable()
    {
      _backButton.onClick.AddListener(BackButtonClicked);
      //ToDo: Load leaderboard and spawn the score prefabs
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