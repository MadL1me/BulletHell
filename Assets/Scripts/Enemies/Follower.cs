using Unity;
using UnityEngine;
using System.Collections;
using System;

public class Follower : Enemy
{
    protected override void AIDecision()
    {
        Move();
    }

    protected override void Attack()
    {
        
    }

    protected override void Move()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        transform.position += Vector3.Normalize(Player.Instance.transform.position - transform.position) * movingSpeed / 1000;
    }
}
