using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Unity;
using UnityEngine;

public class HighDemon : Boss
{

    [SerializeField] private GameObject tridentPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject minionPrefab;


    protected override void AIDecision()
    {
        
    }

    protected override void Attack()
    {
        var attackID = Random.Range(0, 3);
        if (attackID == 0)
            StartCoroutine(TridentAttack());
        else if (attackID == 1)
            StartCoroutine(SpinAttack());
    }

    private IEnumerator SummonAttack()
    {
        isAttacking = true;
        for (int i = 0; i<5; i++)
        {
            var minion = Instantiate(minionPrefab);
            minion.transform.position = transform.position + new Vector3(-1, -1);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }

    private IEnumerator TridentAttack()
    {
        isAttacking = true;
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }

    private IEnumerator SpinAttack()
    {
        isAttacking = true;
        yield return new WaitForFixedUpdate();
        isAttacking = false;
    }


    protected override void Move()
    {
        
    }
}

