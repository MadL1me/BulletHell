using Unity;
using UnityEngine;
using System;
using System.Collections;

public class Bullet : Projectile  
{
    public Vector2 MoveDirection;
    protected Collider2D[] colliders;
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

    protected override void Awake()
    {
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Create(BulletElement blt)
    {
        this.Damage = blt.Damage;
        this.Speed = blt.Speed;
        bulletColor = blt.bulletColor;
    }

    private void Start()
    {
        spriteRenderer.color = bulletColor;
    }

    protected virtual void FixedUpdate()
    {
        transform.position += new Vector3(MoveDirection.x, MoveDirection.y) * _speed * Time.fixedDeltaTime;
    }

    public override IEnumerator Hit()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }

    protected override Vector2 GetTrajectory(float delta)
    {
        var vec = new Vector2(100,100);
        return vec * delta;
    }
}