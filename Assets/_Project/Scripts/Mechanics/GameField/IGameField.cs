﻿using UnityEngine;
using Zenject;

namespace Mechanics.GameField
{
  internal interface IGameField
  {
    float ClampPlayerPosition(float position);
    bool IsOutsideBounds(Vector3 position);
  }
}