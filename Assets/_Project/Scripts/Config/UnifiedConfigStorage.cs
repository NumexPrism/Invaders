using Mechanics.Field;
using Mechanics.Player;
using Mechanics.Projectiles;
using UnityEngine;

namespace Config
{
  //Game designers with whom are worked with were always obsessed with "having one place with all the configs"

  [CreateAssetMenu(menuName = "Configurations/Config Storage", fileName = "Configuration", order = 0)]
  public class UnifiedConfigStorage : ScriptableObject 
  {
    public PlayerShipConfig PlayerShip;
    public GameFieldConfig Field;
    public ProjectileConfig PlayerProjectileConfig;
    public ProjectileConfig EnemyProjectileConfig;
    public EnemyWaveConfig enemiesWave;
  }
}
