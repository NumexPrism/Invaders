using Installers.Project.UI.UiPanel;
using LeaderBoard;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.LeaderBoard
{
  [RequireComponent(typeof(LeaderBoardViewMonoInstaller))]
  internal class LeaderBoardView: BaseUIView
  {
    [Inject] private Button _backButton;//Yagni principle at work: I could have added ButtonId, if since the view only has one button, i will not.
    [Inject] private Image _spinner;
    [Inject] private ILeaderBoardAdapter _leaderBoard;
    [Inject] private ScoreWidget.Factory _widgetFactory;

    private async void OnEnable()
    {
      _backButton.onClick.AddListener(BackButtonClicked);
      _spinner.gameObject.SetActive(true);
      var leadersData = await _leaderBoard.LoadScoresOrdered();
      _spinner.gameObject.SetActive(false);
      foreach (var datum in leadersData)
      {
        _widgetFactory.Create(datum);
      }
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