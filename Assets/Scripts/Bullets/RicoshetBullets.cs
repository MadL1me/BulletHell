using Unity;
using UnityEngine;
using System;
using System.Collections;

public class RicoshetBullets : Bullet
{

    private int ricoshetCount = 0;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        MoveDirection = Vector3.Reflect(MoveDirection, collision.contacts[0].normal);
        ricoshetCount++;

        if (collision.gameObject.layer == 11)
            StartCoroutine(Hit());
   
        if (ricoshetCount == 3)
        {
            Destroy(gameObject);
        }
    }
}
