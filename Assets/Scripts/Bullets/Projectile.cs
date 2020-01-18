using Unity;
using UnityEngine;
using System;
using System.Collections;


public abstract class Projectile : MonoBehaviour
{
    public float Speed { get => _speed; set => _speed = value; }
    [SerializeField] protected float _speed;
    public float Damage { get => _damage; set => _damage = value; }
    [SerializeField] protected float _damage;
    
    protected SpriteRenderer spriteRenderer;
    protected abstract Vector2 GetTrajectory(float delta);
    protected Color bulletColor;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            StartCoroutine(Hit());
        Debug.Log("emeny bullet collision");
    }

    public virtual IEnumerator Hit()
    {
        yield return new WaitForFixedUpdate();
        Destroy(this);
    }
}