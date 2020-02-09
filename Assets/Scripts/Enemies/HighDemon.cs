using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Unity;
using UnityEngine;

public class HighDemon : Boss
{
    public static HighDemon Instance;

    [SerializeField] private GameObject tridentPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject minionPrefab;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void FixedUpdate()
    {
        if (_isActive)
            AIDecision();
    }

    protected override void AIDecision()
    {
        if (!_isAttacking)
            Attack();
        if (!isMoving)
            Move();
    }

    protected override void Attack()
    {
        var attackID = UnityEngine.Random.Range(0, 3);
        if (attackID == 0)
            StartCoroutine(TridentAttack(-3.5f, 3.5f, 28f, 40f));
        else if (attackID == 1)
            StartCoroutine(SpinAttack());
        else if (attackID == 2)
            StartCoroutine(SplashCircleAttack());
    }
    protected override void Move()
    {
        StartCoroutine(MoveSin(5));
    }

    private IEnumerator SummonAttack()
    {
        _isAttacking = true;
        for (int i = 0; i<5; i++)
        {
            var minion = Instantiate(minionPrefab);
            minion.transform.position = transform.position + new Vector3(-1, -1);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        yield return new WaitForFixedUpdate();
        _isAttacking = false;
    }

    private IEnumerator TridentAttack(float xmin, float xmax, float ymin, float ymax)
    {
        _isAttacking = true;

        for (int i = 0; i<10; i++)
        {
            var trident = Instantiate(tridentPrefab);
            trident.transform.position = new Vector2(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
            yield return new WaitForSecondsRealtime(0.3f);
        }

        yield return new WaitForSecondsRealtime(3);
        _isAttacking = false;
    }

    private IEnumerator MoveToCenter(Vector3 centerPosition, float moveSpeed)
    {
        isMoving = true;
        var pos = transform.position;

        for (float i = 0; i < 1; i += _movingSpeed)
        {
            transform.position = Vector3.Lerp(pos, centerPosition, i);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator SplashCircleAttack()
    {
        _isAttacking = true;
        var angle = 0;
        
        for (int j = 0; j<4; j++)
        {
            for (int i = 0; i < 60; i++)
            {
                var obj = Instantiate(bulletPrefab);
                obj.transform.position = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) + transform.position.x, Mathf.Sin(angle * Mathf.PI / 180) + transform.position.y);
                obj.GetComponent<Bullet>().MoveDirection = obj.transform.position - transform.position;
                obj.GetComponent<Bullet>().MoveDirection.Normalize();
                angle += 6;
            }

            yield return new WaitForSecondsRealtime(1f);
        }
      
       _isAttacking = false;
    }

    private IEnumerator SpinAttack()
    {
        _isAttacking = true;
        StartCoroutine(MoveToCenter(new Vector3(0,32,0), 0.1f));
        isMoving = true;
        yield return new WaitForSecondsRealtime(2);

        float alpha = 0f;

        for (int i = 0; i < 150; i++)
        {
            var bullet = Instantiate(bulletPrefab);
            Debug.Log(new Vector3(Mathf.Cos(Mathf.PI * alpha/180), Mathf.Sin(Mathf.PI * alpha/180)));
            bullet.transform.position = new Vector3(Mathf.Cos(Mathf.PI * alpha/180) + transform.position.x, Mathf.Sin(Mathf.PI * alpha/180) + transform.position.y);
            bullet.GetComponent<Bullet>().MoveDirection = bullet.transform.position - transform.position;
            bullet.GetComponent<Bullet>().MoveDirection.Normalize();
            alpha += 115f;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        yield return new WaitForFixedUpdate();
        isMoving = false;
        _isAttacking = false;
    }


    protected IEnumerator MoveSin(float distance)
    {
        isMoving = true;
        Debug.Log("Started moving");
        var m = 100;

        for (float i = 0; i <= distance; i += movingSpeed)
        {
            transform.position = new Vector3(transform.position.x + movingSpeed, transform.position.y + Mathf.Sin(i)/m);
            yield return new WaitForFixedUpdate();
        }

        for (float i = distance; i >= -distance; i -= movingSpeed)
        {
            transform.position = new Vector3(transform.position.x - movingSpeed, transform.position.y + Mathf.Sin(i) /m);
            yield return new WaitForFixedUpdate();
        }

        for (float i = -distance; i <= 0; i += movingSpeed)
        {
            transform.position = new Vector3(transform.position.x + movingSpeed, transform.position.y + Mathf.Sin(i)/ m);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
        yield break;
    }

    protected override IEnumerator Introduce()
    {


        yield break;
    }
}

