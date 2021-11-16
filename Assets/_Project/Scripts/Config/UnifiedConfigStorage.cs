using Mechanics.GameField;
using Mechanics.Player;
using Mechanics.Projectiles;
using UnityEngine;

namespace Config
{
  [CreateAssetMenu(menuName = "Configurations/Config Storage", fileName = "Configuration", order = 0)]
  public class UnifiedConfigStorage : ScriptableObject 
  {
    public PlayerShipConfig PlayerShip;
    public GameFieldConfig Field;
    public ProjectileConfig PlayerProjectileConfig;
    public ProjectileConfig EnemyProjectileConfig;
  }
}
