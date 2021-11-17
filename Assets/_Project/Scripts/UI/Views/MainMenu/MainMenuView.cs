using DependencyInjection.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.MainMenu
{
  [RequireComponent(typeof(MainMenuViewMonoInstaller))]
  internal class MainMenuView: BaseUIView
  {
    // This place here, is where DI shows it's worst. 
    // I had to create another installer script and add the references there, also introduced an entity which is ButtonId
    // and I also had to add an context script to the same GO so the installer would work.
    //
    // I could have just added references to this monobehaviour and drag'n'drop them in editor. It would be much cleaner, and way faster.
    // and I can't even test these buttons because they are 3rd party, and don't provide a way to mock the clicks
    // so, the DI is just a 100% boilerplate here.

    [Inject(Id = ButtonId.Start)] private Button _startButton;
    [Inject(Id = ButtonId.HighScore)] private Button _leaderBoardButton;
    [Inject(Id = ButtonId.Exit)] private Button _exitButton;

    private void OnEnable()
    {
      _startButton.onClick.AddListener(StartButtonClicked);
      _leaderBoardButton.onClick.AddListener(LeaderBoardButtonClicked);
      _exitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void OnDisable()
    {
      _startButton.onClick.RemoveListener(StartButtonClicked);
      _leaderBoardButton.onClick.RemoveListener(LeaderBoardButtonClicked);
      _exitButton.onClick.RemoveListener(ExitButtonClicked);
    }

    private void StartButtonClicked()
    {
      UiFacade.ShowNextView();
    }

    private void LeaderBoardButtonClicked()
    {
      UiFacade.ShowLeaderBoard();
    }

    private void ExitButtonClicked()
    {
      Application.Quit();
    }
  }
}