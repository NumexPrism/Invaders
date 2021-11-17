namespace UI
{
  internal interface IUiDebug
  {
    #if UNITY_EDITOR
    void ForceSwitchToGameUi();
    #endif
  }
}