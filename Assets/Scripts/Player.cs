using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public enum BulletTypes
{
    Standart,
    Recoshet,
    Silver,
    Hellfire
}


public class Player : MonoBehaviour
{
    public static Player Instance;

    public float MovingSpeed { get => _movingSpeed; set => _movingSpeed = value; }
    [SerializeField] private float _movingSpeed = 300f;
    public float DashingSpeed { get => _dashingSpeed; set => _dashingSpeed = value; }
    [SerializeField] private float _dashingSpeed = 200f;
    public int LifeCount 
    { get => _lifeCount;
        set
        {
            _lifeCount = value;
            UIManager.Instance.ChangeHeartsCount(value, _lifeCapacity);
        }
    }
    [SerializeField] private int _lifeCount = 3;

    public int LifeCapacity { get => _lifeCapacity; 
        set
        {
            _lifeCapacity = value;
            if (_lifeCount > _lifeCapacity)
                _lifeCount = value;
            UIManager.Instance.ChangeHeartsCapacity(value);
        }
    }
    [SerializeField] private int _lifeCapacity = 3;


    [SerializeField] private float _dashTimeout = 0.5f;
    [SerializeField] private float _shootTimeout = 0.1f;
    [SerializeField] private float _spread;
    [SerializeField] private float _reloadSpeed;
    //  [SerializeField] private float 


    [SerializeField] GameObject simpleBltPrefab;

    [SerializeField] private List<BulletTypes> _bulletElements;
    [SerializeField] private List<Bullet> bulletsInRound;
    private CapsuleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movement = new Vector2();

    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool isInvisible = false;
    [SerializeField] private bool isReloading = false;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _bulletElements = new List<BulletTypes>();
        bulletsInRound = new List<Bullet>();

        if (Instance == null)
            Instance = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollisions(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollisions(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var obj = collision.transform.gameObject;
        if (obj.CompareTag("Bullet") && (isInvisible || isDashing))
        {
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), _collider, false);
        }
    }

    private void CheckCollisions(Collision2D collision)
    {
        var obj = collision.transform.gameObject;

        if (obj.CompareTag("Enemy") && !(isInvisible || isDashing))
        {
            StartCoroutine(GetDamage());
        }
        else if (obj.CompareTag("Bullet") && !(isInvisible || isDashing))
        {
            StartCoroutine(GetDamage());
            StartCoroutine(obj.GetComponent<Projectile>().Hit());
        }
    }

    private void Update()
    {
        if (!isDashing)
        {
            Move();

            if (Input.GetMouseButtonDown(0))
                StartCoroutine(Shoot());
         
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(Dash());
     
            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
                StartCoroutine(Reload());
            if (Input.GetKeyDown(KeyCode.B))
                StartCoroutine(SmartCamera.Instance.ChangeSize(40, 10));
        }
    }

    private void AddBullet(BulletTypes bulletTypes)
    {
        if (_bulletElements.Count >= 5)
            return;

        _bulletElements.Add(bulletTypes);
        UIManager.Instance.AddBullet(bulletTypes, _bulletElements);
    }

    private IEnumerator GetDamage()
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);
        LifeCount--;
        isInvisible = true;

        for (int j = 0; j < 5; j++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        isInvisible = false;
        Physics2D.IgnoreLayerCollision(8, 10, false);
    }


    private IEnumerator Dash() 
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);
       
        isDashing = true;
        _spriteRenderer.color = Color.blue;
        var directionVector = GetDirection().normalized * _dashingSpeed * Time.fixedDeltaTime;

        for (int i = 0; i<30; i++) 
        {
            transform.position += new Vector3(directionVector.x, directionVector.y); ;
            yield return new WaitForFixedUpdate();
        }
        _spriteRenderer.color = Color.white;
        isDashing = false;

        Physics2D.IgnoreLayerCollision(8, 10, false);
    }

    private IEnumerator UseActiveItem() 
    {
        yield break;
    }

    private IEnumerator Shoot() 
    {
        if (bulletsInRound.Count == 0)
        {
            yield break;
        }
        bulletsInRound.RemoveAt(0);
        UIManager.Instance.ShootBullet(bulletsInRound);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos = mousePos - transform.position;
        var obj = Instantiate(simpleBltPrefab);
        obj.transform.position = transform.position + mousePos.normalized/2;
        obj.GetComponent<Bullet>().MoveDirection = mousePos.normalized;
        yield break;
    }
    
    private IEnumerator Reload() 
    {
        Debug.Log("Reloading starter");
        isReloading = true;
        _bulletElements.Clear();
        foreach (Image renderer in UIManager.Instance.bulletPlaces)
            renderer.color = Color.black;

        yield return new WaitForFixedUpdate();

        while (true)
        {
            if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButton(0)) &&  _bulletElements.Count != 0)
            {
                Debug.Log("OMG");
                break;
            }

            if (Input.GetKeyDown(KeyCode.Z))
                AddBullet(BulletTypes.Standart);

            else if (Input.GetKeyDown(KeyCode.X))
                AddBullet(BulletTypes.Recoshet);

            else if (Input.GetKeyDown(KeyCode.C))
                AddBullet(BulletTypes.Silver);

            else if (Input.GetKeyDown(KeyCode.V))
                AddBullet(BulletTypes.Hellfire);

            yield return new WaitUntil(CheckBtn);
        }
       
        yield return new WaitForSecondsRealtime(0.1f);

        MakeBulletSet();
        isReloading = false;
        yield break;

        bool CheckBtn() => Input.anyKeyDown || Input.GetMouseButtonDown(0);
    }


    private void MakeBulletSet()
    {
        foreach(BulletTypes type in _bulletElements)
        {
            if (type == BulletTypes.Standart)
                bulletsInRound.Add(new Bullet(10, 10, Color.white));
            else if (type == BulletTypes.Silver)
                bulletsInRound.Add(new Bullet(20, 20, Color.blue));
            else if (type == BulletTypes.Hellfire)
                bulletsInRound.Add(new Bullet(5, 50, Color.red));
            else if (type == BulletTypes.Recoshet)
                bulletsInRound.Add(new Bullet(10, 10, Color.white));
        }
    }

    private void Move() 
    {
        _movement = GetDirection();
        var moveVector = _movement.normalized * _movingSpeed  * Time.fixedDeltaTime;
        transform.position += new Vector3(moveVector.x, moveVector.y);
    }

    private Vector2 GetDirection()
    {
        var vector = new Vector2();
        vector.x = Input.GetAxisRaw("Horizontal");
        vector.y = Input.GetAxisRaw("Vertical");
        return vector;
    }
}
