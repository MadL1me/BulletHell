using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected virtual float healthAmmount
    {
        get => _healthAmmount;
        set
        {
            _healthAmmount = value;
            if (_healthAmmount < 0)
                StartCoroutine(Die());
        }
    }
    [SerializeField] protected float _healthAmmount = 100;
    protected float movingSpeed => _movingSpeed;
    [SerializeField] protected float _movingSpeed = 200;
    [SerializeField] protected string _name = "name";
    [SerializeField] protected float _attackTimeout = 1f;

    protected SpriteRenderer _spriteRenderer;
    protected bool _isAttacking = false;
    [SerializeField] protected bool isMoving = false;

    protected abstract void Attack();
    protected abstract void AIDecision();
    protected abstract void Move();


    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        AIDecision();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        Debug.Log("Enemy Collision!");
        if (obj.layer == 12)
            StartCoroutine(GetHit(obj));
    }

    protected virtual IEnumerator GetHit(GameObject obj)
    {
        var Bullet = obj.GetComponent<Projectile>();
        healthAmmount -= Bullet.Damage;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(0.1f);
        _spriteRenderer.color = Color.white;
    }

    protected virtual IEnumerator Die()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}