using Unity;
using UnityEngine;
using System;


public abstract class Projectile : MonoBehaviour
{
    protected float _speed;
    protected float _damage;
    protected abstract Vector2 GetTrajectory(float delta);
}