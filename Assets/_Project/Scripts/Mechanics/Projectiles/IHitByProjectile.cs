namespace Mechanics.Projectiles
{
  public interface IHitByProjectile
  {
    Party Party { get; }
    void RespondToHit();
  }
}