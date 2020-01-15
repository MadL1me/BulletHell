using Unity;
using UnityEngine;
using System;


public class PlayerBullet : Projectile 
{
    public void CreateBulletEffect() 
    {


    }

    public override void _PhysicsProcess(float delta)
    {
        var direction = GetTrajectory(delta);
    }

    protected override Vector2 GetTrajectory(float delta)
    {
        var vec = new Vector2(100,100);
        return vec * delta;
    }
}