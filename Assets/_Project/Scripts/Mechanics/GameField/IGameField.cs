using Zenject;

namespace Mechanics.GameField
{
  internal interface IGameField
  {

    float ClampPlayerPosition(float position);
  }
}