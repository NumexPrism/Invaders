using Config;
using Mechanics.Enemy;
using Mechanics.Field;
using Mechanics.Player;
using Mechanics.Projectiles;
using UI;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
  [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
  class ProjectContextInstaller: ScriptableObjectInstaller<ProjectContextInstaller>
  {
    [SerializeField] private UiFacade uiPrefab; 
    [SerializeField] private Projectile ProjectilePrefab;
    [SerializeField] private SimpleEnemy EnemyPrefab;
    [SerializeField] private UnifiedConfigStorage ConfigStorage;

    public override void InstallBindings()
    {
      Container.BindInterfacesTo<UiFacade>().FromComponentInNewPrefab(uiPrefab).AsSingle();

      Container.BindFactory<ProjectileLaunchParameters, Projectile, Projectile.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(2)
          .FromComponentInNewPrefab(ProjectilePrefab)
          .UnderTransformGroup("Projectiles"));

      Container.BindFactory<EnemySpawnParameters, SimpleEnemy, SimpleEnemy.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(32)
          .FromComponentInNewPrefab(EnemyPrefab)
          .UnderTransformGroup("Enemies"));

      Container.Bind<IPlayerShipConfig>()
        .FromInstance(ConfigStorage.PlayerShip);
      Container.Bind<IGameFieldConfig>()
        .FromInstance(ConfigStorage.Field);
      Container.Bind<IProjectileConfig>()
        .WithId(Party.Player)
        .FromInstance(ConfigStorage.PlayerProjectileConfig);
      Container.Bind<IProjectileConfig>()
        .WithId(Party.Enemy)
        .FromInstance(ConfigStorage.EnemyProjectileConfig);

      Container.Bind<EnemyWave>().AsCached();
    }
  }
}
