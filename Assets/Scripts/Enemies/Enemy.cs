using Unity;
using UnityEngine;
using System;


public abstract class Enemy
{
    protected float healthAmmount {get; set;} = 100;
    protected float movingSpeed {get; set;} = 200;
    protected abstract void Attack();
}