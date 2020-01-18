using Unity;
using UnityEngine;
using System;
using System.Collections;

public class Bullet : Projectile 
{
    public Vector2 MoveDirection;

    public void CreateBulletEffect() 
    {

       
    }

    public Bullet()
    {

    }

    public Bullet(float speed, float damage, Color color)
    {
        this.Damage = damage;
        this.Speed = speed;
        bulletColor = color;
    }

    private void Update()
    {
        transform.position += new Vector3(MoveDirection.x, MoveDirection.y) * _speed * Time.fixedDeltaTime;
    }


    public override IEnumerator Hit()
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("PlayerBullet collision detected!");
        Destroy(gameObject);
    }

    protected override Vector2 GetTrajectory(float delta)
    {
        var vec = new Vector2(100,100);
        return vec * delta;
    }
}