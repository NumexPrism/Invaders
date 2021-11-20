using Cysharp.Threading.Tasks;

namespace UI
{
  internal interface IUiView
  {
    UniTask Show();
    UniTask Hide();
  }
}