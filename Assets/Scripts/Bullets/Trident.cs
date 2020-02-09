using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
public class Trident : Bullet
{
    private bool spawning = false;

    private void Start()
    {
        StartCoroutine(SpawnAnim());
    }

    private IEnumerator SpawnAnim()
    {
        spawning = true;
        colliders[0].enabled = false;
        colliders[1].enabled = false;
        var rotatingSpeed = 10f;
        var spawnSpeed = 10f;

        //rotating and sizing up
        for (float i = 0; i<2; i+=Time.fixedDeltaTime)
        {
            transform.Rotate(Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0,0,360*8), i/2));
            transform.localScale += new Vector3(0.03f, 0.03f,0);
            yield return new WaitForFixedUpdate();
        }

        colliders[0].enabled = true;
        colliders[1].enabled = true;

        //watching to player
        for (float i = 0; i<1; i+= Time.fixedDeltaTime)
        {        
            transform.LookAt(Player.Instance.transform.position);
            
            if (transform.rotation.eulerAngles.y > 180)
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.x - transform.eulerAngles.y);
            else
                transform.rotation = Quaternion.Euler(0, 0, -transform.eulerAngles.x + transform.eulerAngles.y + 180);
            
            yield return new WaitForFixedUpdate();
        }
        spawning = false;
        MoveDirection = transform.up;
    }

    private void FixedUpdate()
    {
        if (!spawning)
            transform.position += new Vector3(MoveDirection.x, MoveDirection.y) * _speed * Time.fixedDeltaTime;
    }


}

