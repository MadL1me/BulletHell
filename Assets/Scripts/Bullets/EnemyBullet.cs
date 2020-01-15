using Unity;
using UnityEngine;
using System;

public class EnemyBullet : Projectile 
{
    protected override Vector2 GetTrajectory(float delta)
    {
        var vec = new Vector2(10,10);
        return vec * delta;
    }
}