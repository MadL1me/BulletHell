using Unity;
using UnityEngine;
using System;


public abstract class Boss : Enemy
{

    protected override void Awake()
    {
        
    }

    protected virtual void BossSpawn()
    {
        StartCoroutine(SmartCamera.Instance.ChangeSize(40, 10));
        StartCoroutine(UIManager.Instance.InitBossBar());
    }

    protected override void FixedUpdate()
    {
        
    }

}