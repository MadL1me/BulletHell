using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;
using UnityEditor;


public class DemonicBullets : Bullet
{

    protected override void FixedUpdate()
    {
        transform.position += (GetClosestEnemyPosition()-transform.position).normalized * Time.fixedDeltaTime * Speed;
    }

    public Vector3 GetClosestEnemyPosition()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        var pos = Vector3.zero;
        var minDistance = 1000f;

        foreach(GameObject obj in objects)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) <= minDistance)
            {
                minDistance = Vector3.Distance(transform.position, obj.transform.position);
                pos = obj.transform.position;
            }
        }
        return pos;
    }
}