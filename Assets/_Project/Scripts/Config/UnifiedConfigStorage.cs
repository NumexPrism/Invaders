using Mechanics.GameField;
using Mechanics.Player;
using UnityEngine;

namespace DependencyInjection
{
  [CreateAssetMenu(menuName = "Configurations/Config Storage", fileName = "Configuration", order = 0)]
  public class UnifiedConfigStorage : ScriptableObject, IPlayerShipConfig, IGameFieldConfig
  {
    public PlayerShipConfig PlayerShip;
    public GameFieldConfig Field;

    public float Speed => PlayerShip.speed;
    public float ShootDelay => PlayerShip.shootDelay;
    public float Width => Field.Width;
  }
}
