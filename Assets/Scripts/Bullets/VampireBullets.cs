using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class VampireBullets : Bullet
{

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Wall")
            Hit();
    }

    public new void Hit()
    {
        StartCoroutine(base.Hit());       
        var player = Player.Instance;
        var hpRestoreChance = 0;

        if (Player.Instance.Lives == 2)
            hpRestoreChance = 35;
        else
            hpRestoreChance = 60;

        var result = UnityEngine.Random.Range(0, 100);
        if (result <= hpRestoreChance)
            Player.Instance.Lives++;
    }


}