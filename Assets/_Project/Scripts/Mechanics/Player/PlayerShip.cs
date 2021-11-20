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
    [Inject] private Renderer _renderer;

    [Inject] private Blinker _blinker;

    [Inject(Id = Party.Player)] private IProjectileConfig _projectileConfig;

    public event Action Damaged;

    //here's another approach to reacting to field changes - via thick setter
    private bool _isInvincible = false;
    public bool IsInvincible
    {
      get => _isInvincible;
      set
      {
        if (_isInvincible != value)
        {
          InvincibilityChangedTo(value);
          _renderer.material.SetColor(BaseColor, _defaultMaterialColor);
          _isInvincible = value;
        }
      }
    }

    private void InvincibilityChangedTo(bool isNowInvincible)
    {
      if (isNowInvincible)
      {
        _blinker.Start(
          _config.BlinkDuration,
          () => _renderer.material.SetColor(BaseColor, _config.BlinkColor),
          () => _renderer.material.SetColor(BaseColor, _defaultMaterialColor)
        );
      }
      else
      {
        _blinker.Stop();
      }
    }

    private float _speed;
    private Task _shootTimeoutTask;
    private Color _defaultMaterialColor;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public Party Party => Party.Player;

    public void RespondToHit()
    {
      if (!IsInvincible)
      {
        Damaged?.Invoke();
      }
    }

    public bool IsAllowedToShoot => _shootTimeoutTask == null || _shootTimeoutTask.IsCompleted;

    private void Start()
    {
      _defaultMaterialColor = _renderer.material.GetColor(BaseColor);
    }

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
      ProcessMovement();
    }

    private void ProcessMovement()
    {
      float deltaMove = (_speed * Time.deltaTime);
      Vector3 previousPosition = transform.position;
      float finalPositionX = _gameField.ClampPlayerPosition(previousPosition.x + deltaMove);
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