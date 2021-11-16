using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Input;
using Mechanics.GameField;
using Mechanics.Projectiles;
using UnityEngine;
using Zenject;

namespace Mechanics.Player
{
  public class PlayerShip : MonoBehaviour, IPlayerShip
  {
    [Inject] private IGameInput _input;
    [Inject] private IPlayerShipConfig _config;
    [Inject] private Projectile.Factory _projectileFactory;
    [Inject] private IGameField _gameField;
    [Inject(Id = Party.Player)] private IProjectileConfig _projectileConfig;

    private float _speed;
    private Task _shootTimeoutTask;

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