using System;

namespace UI
{
  internal interface IUiFacade
  {
    bool ShowNextView();
    bool ShowPreviousView();
    bool ShowLeaderBoard();

    event Action GameStopped;
  }
}