using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Input;
using Mechanics.Field;
using Mechanics.Projectiles;
using UnityEngine;
using Zenject;

namespace Mechanics.Player
{
  [RequireComponent(typeof(Collider))]
  public class PlayerShip : MonoBehaviour, IPlayerShip, IHitByProjectile
  {
    [Inject] private IGameInput _input;
    [Inject] private IPlayerShipConfig _config;
    [Inject] private Projectile.Factory _projectileFactory;
    [Inject] private IGameFieldConfig _gameField;
    [Inject(Id = Party.Player)] private IProjectileConfig _projectileConfig;

    public event Action Damaged;

    public bool IsInvincible { get; set; } = false;

    private float _speed;
    private Task _shootTimeoutTask;

    public Party Party => Party.Player;

    public void RespondToHit()
    {
      if (!IsInvincible)
      {
        Damaged?.Invoke();
      }
    }

    public bool IsAllowedToShoot => _shootTimeoutTask == null || _shootTimeoutTask.IsCompleted;

    private void OnEnable()
    {
      _input.MoveStarted += OnMoveStarted;
      _input.MoveStopped += OnMoveStopped;
      _input.Shoot += OnShoot;
    }

    private void OnDisable()
    {
      _input.Shoot -= OnShoot;
      _input.MoveStopped -= OnMoveStopped;
      _input.MoveStarted -= OnMoveStarted;
    }

    private void Update()
    {
      float deltaMove = (_speed * Time.deltaTime);
      Vector3 previousPosition = transform.position;
      float finalPositionX = _gameField.ClampPlayerPosition(previousPosition.x+deltaMove);
      transform.position = new Vector3(finalPositionX, previousPosition.y, previousPosition.z);
    }

    private void OnShoot()
    {
      if (IsAllowedToShoot)
      {
        _shootTimeoutTask = WaitForTimeout();

        var parameters = new ProjectileLaunchParameters(transform.position, Party.Player, _projectileConfig);

        _projectileFactory.Create(parameters);
      }
    }

    async Task WaitForTimeout()
    {
      await UniTask.Delay(TimeSpan.FromSeconds(_config.ShootDelay), DelayType.DeltaTime);
    }

    private void OnMoveStopped()
    {
      _speed = 0.0f;
    }

    private void OnMoveStarted(float direction)
    {
      _speed = direction * _config.Speed;
    }
  }
}