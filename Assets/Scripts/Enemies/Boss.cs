using Unity;
using UnityEngine;
using System;
using System.Collections;

public abstract class Boss : Enemy
{
    public Collider2D BossStartCollider;
    protected bool _isActive = false;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        Debug.Log("Enemy Collision!");
        if (obj.layer == 12)
            GetHitBoss(obj);
    }


    public virtual void BossSpawn()
    {
        //StartCoroutine(SmartCamera.Instance.ChangeSize(16, 10));
        StartCoroutine(UIManager.Instance.BossHealthBar());
        _isActive = true;
    }

    protected abstract IEnumerator Introduce();

    protected void GetHitBoss(GameObject obj)
    {
        StartCoroutine(base.GetHit(obj));
        UIManager.Instance.BossHealthBarChange(_healthAmmount);
    }

}