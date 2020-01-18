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
        SmartCamera.Instance.ChangeSize(40, 10);
    }

    protected override void FixedUpdate()
    {
        
    }

}