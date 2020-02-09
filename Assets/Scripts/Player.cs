using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public enum BulletTypes
{
    Standart,
    Blood,
    Silver,
    Hellfire,
}


public class Player : MonoBehaviour
{
    public static Player Instance;

    public float MovingSpeed { get => _movingSpeed; set => _movingSpeed = value; }
    [SerializeField] private float _movingSpeed = 300f;
    public float DashingSpeed { get => _dashingSpeed; set => _dashingSpeed = value; }
    [SerializeField] private float _dashingSpeed = 200f;
    public int Lives 
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
    [SerializeField] private float _spread;
    [SerializeField] private float _reloadSpeed;
    //  [SerializeField] private float 


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject demonicBulletPrefab;
    [SerializeField] GameObject ricoshetBulletPrefab;
    [SerializeField] GameObject ghostBulletPrefab;
    [SerializeField] GameObject vampireBulletsPrefab;

    [SerializeField] private List<BulletTypes> _bulletElements;
    [SerializeField] private List<BulletElement> _bulletsInRound;
    private CapsuleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movement = new Vector2();

    [SerializeField] private bool _isDashing = false;
    [SerializeField] private bool _isInvisible = false;
    [SerializeField] private bool _isReloading = false;
    private bool _isTimeout = false;

    private Animator animator;
    public bool IsWatchingUp = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _bulletElements = new List<BulletTypes>();
        _bulletsInRound = new List<BulletElement>();
        animator = GetComponent<Animator>();

        if (Instance == null)
            Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == HighDemon.Instance.BossStartCollider)
        {
            HighDemon.Instance.BossSpawn();
        }

        if (collision == TrainingManager.Instance.Colliders[0])
        {
            TrainingManager.Instance.Introduce();
            Destroy(TrainingManager.Instance.Colliders[0]);
        }

        if (collision == TrainingManager.Instance.Colliders[1])
        {
            TrainingManager.Instance.LearnToDash();
            Destroy(TrainingManager.Instance.Colliders[1]);
        }

        if (collision == TrainingManager.Instance.Colliders[2])
        {
            TrainingManager.Instance.LearnToShoot();
            Destroy(TrainingManager.Instance.Colliders[2]);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollisions(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollisions(collision);
    }

    private void CheckCollisions(Collision2D collision)
    {
        var obj = collision.transform.gameObject;

        if (obj.CompareTag("Enemy") && !(_isInvisible || _isDashing))
        {
            StartCoroutine(GetDamage());
        }
        else if (obj.CompareTag("Bullet") && !(_isInvisible || _isDashing))
        {
            StartCoroutine(GetDamage());
            StartCoroutine(obj.GetComponent<Projectile>().Hit());
        }
    }

    float time = 0;
   

    private void FixedUpdate()
    {
        if (!_isDashing && !GameManager.isPause)
        {
            Move();
        }
    }

    private void Update()
    {
        if (!_isDashing && !GameManager.isPause)
        {
            if (Input.GetMouseButtonDown(0))
                StartCoroutine(Shoot());
         
            if (Input.GetKeyDown(KeyCode.Space) && !_isTimeout && GetDirection().magnitude >= 0.001d)
                StartCoroutine(Dash());
     
            if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
                StartCoroutine(Reload());
            
            if (Input.GetKeyDown(KeyCode.B))
                StartCoroutine(SmartCamera.Instance.ChangeSize(40, 10));

            PlayAnimations();
        }
    }

    private void PlayAnimations()
    {
        var pos = Input.mousePosition;
        var move = GetDirection();
        var animName = "PlayerIdleFront";
        var angle = GetAngle();

        IsWatchingUp = false;

        if (pos.y >= Screen.height / 2)
        {
            if (angle >= 160 || angle <= 20)
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleSideR";
                else
                    animName = "PlayerMoveSideUR";

            }
            else if (angle >= 125 || angle <= 55)
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleSideUpR";
                else
                    animName = "PlayerMoveSideUpR";

                IsWatchingUp = true;
            }
            else
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleBack";
                else
                    animName = "PlayerMoveUp";

                IsWatchingUp = true;
            }
        }
        else 
        {
            if (angle >= 160 || angle <= 20)
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleSideR";
                else
                    animName = "PlayerMoveSideR";

            }
            else if (angle >= 125 || angle <= 55)
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleSideR";
                else
                    animName = "PlayerMoveSideR";

            }
            else
            {
                if (move.magnitude <= 0.01)
                    animName = "PlayerIdleFront";
                else
                    animName = "PlayerMoveDown";

            }
        }

        if (pos.x >= Screen.width/2)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;


        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        animator.Play(animName);

        float GetAngle() =>
           180 / Mathf.PI * Mathf.Acos(Vector3.Dot(Vector3.right, new Vector3(pos.x-Screen.width/2,pos.y-Screen.height/2,0))/new Vector3(pos.x - Screen.width / 2, pos.y - Screen.height / 2, 0).magnitude);
    }

    private void AddBullet(BulletTypes bulletTypes)
    {
        if (_bulletElements.Count >= 5)
           return;

        _bulletElements.Add(bulletTypes);
        UIManager.Instance.AddBullet(bulletTypes, _bulletElements);
        AudioManager.Instance.PlayAddBulletToRoll();
    }


    private IEnumerator GetDamage()
    {
        Physics2D.IgnoreLayerCollision(10, 13, true);
        Lives--;
        _isInvisible = true;

        for (int j = 0; j < 5; j++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        _isInvisible = false;
        Physics2D.IgnoreLayerCollision(10, 13, false);
    }


    private IEnumerator Dash() 
    {
        Physics2D.IgnoreLayerCollision(13, 10, true);
       
        _isDashing = true;
        _spriteRenderer.color = Color.red;
        var directionVector = GetDirection().normalized * _dashingSpeed * Time.fixedDeltaTime;

        for (int i = 0; i<20; i++) 
        {
            transform.position += new Vector3(directionVector.x, directionVector.y); ;
            yield return new WaitForFixedUpdate();
        }

        _spriteRenderer.color = Color.white;
        _isDashing = false;

        Physics2D.IgnoreLayerCollision(13, 10, false);
        _isTimeout = true;
        yield return new WaitForSecondsRealtime(_dashTimeout);
        _isTimeout = false;
    }

    private IEnumerator UseActiveItem() 
    {
        yield break;
    }

    private IEnumerator Shoot()
    {
        if (_bulletsInRound.Count == 0)
        {
            yield break;
        }

        SmartCamera.Instance.CameraShake();
        var element = _bulletsInRound.First();
        _bulletsInRound.RemoveAt(0);
        UIManager.Instance.ShootBullet(_bulletsInRound.Count, _bulletElements.Count - 1);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos = mousePos - Weapon.Instance.GetSpawnPoint();
        var prefab = bulletPrefab;

        if (element.type == ComboType.Double)
        {
            for (int i = 0; i < 2; i++)
            {
                var objd = Instantiate(bulletPrefab);
                objd.transform.position = Weapon.Instance.GetSpawnPoint();
                objd.GetComponent<Bullet>().MoveDirection = mousePos.normalized;
                var bltd = objd.GetComponent<Bullet>();
                bltd.Create(element);
                StartCoroutine(UIManager.Instance.RotateRevolverRoll());
                AudioManager.Instance.PlayShootSound();
                animator.Play("PlayerShoot", animator.GetLayerIndex("ShootLayer"), 0f);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield break;
        }
        else if (element.type == ComboType.Shotgun)
        {
            var rand = UnityEngine.Random.Range(3, 5);

            for (int i = 0; i < rand; i++)
            {
                var randAngle = UnityEngine.Random.Range(-30f, 30f) * Mathf.PI / 180;
                var objd = Instantiate(bulletPrefab);
                objd.transform.position = Weapon.Instance.GetSpawnPoint();
                objd.GetComponent<Bullet>().MoveDirection = new Vector3(mousePos.normalized.x * Mathf.Cos(randAngle) - mousePos.normalized.y * Mathf.Sin(randAngle), mousePos.normalized.x * Mathf.Sin(randAngle) + mousePos.normalized.y * Mathf.Cos(randAngle));
                var bltd = objd.GetComponent<Bullet>();
                bltd.Create(element);
                StartCoroutine(UIManager.Instance.RotateRevolverRoll());
                AudioManager.Instance.PlayShootSound();
                animator.Play("PlayerShoot", animator.GetLayerIndex("ShootLayer"), 0f);
            }
            yield break;
        }
        else if (element.type == ComboType.Vampire)
            prefab = vampireBulletsPrefab;
        else if (element.type == ComboType.Demonic)
            prefab = demonicBulletPrefab;
        else if (element.type == ComboType.Ghost)
            prefab = ghostBulletPrefab;
        else if (element.type == ComboType.Ricoshet)
            prefab = ricoshetBulletPrefab;

        var obj = Instantiate(prefab);
        obj.transform.position = Weapon.Instance.GetSpawnPoint();
        obj.GetComponent<Bullet>().MoveDirection = mousePos.normalized;
        var blt = obj.GetComponent<Bullet>();
        //blt.Create(element);
        StartCoroutine(UIManager.Instance.RotateRevolverRoll());
        AudioManager.Instance.PlayShootSound();
        animator.Play("PlayerShoot", animator.GetLayerIndex("ShootLayer"), 0f);
        yield break;

    }
    
    private IEnumerator Reload() 
    {
        Debug.Log("Reloading starter");
        _isReloading = true;
        _bulletElements.Clear();
        _bulletsInRound.Clear();

        StartCoroutine(UIManager.Instance.RotateRevolverRoll(true));
        foreach (Image renderer in UIManager.Instance.bulletsPlaces)
            renderer.color = Color.black;

        yield return new WaitForFixedUpdate();

        while (true)
        {
            if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButton(0)) && _bulletElements.Count != 0)
            {
                Debug.Log("OMG");
                StartCoroutine(UIManager.Instance.RotateRevolverRoll(true));
                break;
            }

            if (Input.GetKeyDown(KeyCode.Z))
                AddBullet(BulletTypes.Standart);

            else if (Input.GetKeyDown(KeyCode.X))
                AddBullet(BulletTypes.Blood);

            else if (Input.GetKeyDown(KeyCode.C))
                AddBullet(BulletTypes.Silver);

            else if (Input.GetKeyDown(KeyCode.V))
                AddBullet(BulletTypes.Hellfire);

            yield return new WaitUntil(CheckBtn);
        }
       
        yield return new WaitForSecondsRealtime(0.1f);

        AudioManager.Instance.PlayRevolverRollSound();
        MakeBulletSet();
        _isReloading = false;
        yield break;

        bool CheckBtn() => Input.anyKeyDown || Input.GetMouseButtonDown(0);
    }

    private void MakeBulletSet()
    {

        List<BulletTypes> elements = new List<BulletTypes>(_bulletElements);

        if (elements.Count == 5)
        {
            //Demonic
            if (elements.First() == BulletTypes.Hellfire
             && elements.ElementAt(1) == BulletTypes.Blood
             && elements.ElementAt(2) == BulletTypes.Hellfire
             && elements.ElementAt(3) == BulletTypes.Blood
             && elements.ElementAt(4) == BulletTypes.Hellfire)
            {
                UIManager.Instance.ChangeComboText("Demonic Bullets");
                for (int i = 0; i < elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(1, 1, Color.yellow, ComboType.Demonic));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Demonic);
                return;
            }

            //ShotGun
            if (elements.First() == BulletTypes.Standart
             && elements.ElementAt(1) == BulletTypes.Silver
             && elements.ElementAt(2) == BulletTypes.Standart
             && elements.ElementAt(3) == BulletTypes.Blood
             && elements.ElementAt(4) == BulletTypes.Standart)
            {
                UIManager.Instance.ChangeComboText("Shotgun Bullets");
                for (int i = 0; i < elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(30, 5, Color.green, ComboType.Shotgun));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Shotgun);
                return;
            }

            //Ghost
            if (elements.First() == BulletTypes.Hellfire
             && elements.ElementAt(1) == BulletTypes.Standart
             && elements.ElementAt(2) == BulletTypes.Blood
             && elements.ElementAt(3) == BulletTypes.Standart
             && elements.ElementAt(4) == BulletTypes.Silver)
            {
                UIManager.Instance.ChangeComboText("Ghost Bullets");
                for (int i = 0; i < elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(1, 1, Color.magenta, ComboType.Ghost));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Ghost);
                return;
            }

        }
        if (elements.Count >= 4)
        {
            //Vampire
            if (elements.First() == BulletTypes.Blood
             && elements.ElementAt(1) == BulletTypes.Blood
             && elements.ElementAt(2) == BulletTypes.Blood
             && elements.ElementAt(3) == BulletTypes.Blood && elements.Count == 4)
            {
                UIManager.Instance.ChangeComboText("Vampire Bullets");
                for (int i = 0; i < elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(1, 1, Color.blue, ComboType.Vampire));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Vampire);
                return;
            }
        }
        if (elements.Count >= 3)
        {
            // ricoshet
            if (elements.First() == BulletTypes.Standart 
                && elements.ElementAt(1) == BulletTypes.Blood 
                && elements.ElementAt(2) == BulletTypes.Silver)
            {
                UIManager.Instance.ChangeComboText("Ricoshet Bullets");
                for (int i = 0; i<elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(10, 5, Color.blue, ComboType.Ricoshet));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Ricoshet);
                return;
            }

            // double
            if (elements.First() == BulletTypes.Standart
                && elements.ElementAt(1) == BulletTypes.Hellfire
                && elements.Count == 3)
            {
                UIManager.Instance.ChangeComboText("Double Bullets");
                for (int i = 0; i < elements.Count; i++)
                {
                    _bulletsInRound.Add(new BulletElement(3, 3, Color.white, ComboType.Double));
                }
                UIManager.Instance.ChangeComboBullets(ComboType.Double);
                return;
            }
        }
        if (elements.Count >= 2)
        {

        }

        foreach(BulletTypes type in _bulletElements)
        {
            if (type == BulletTypes.Standart)
                _bulletsInRound.Add(new BulletElement(2, 2, Color.white, ComboType.Standart));
            else if (type == BulletTypes.Silver)
                _bulletsInRound.Add(new BulletElement(10, 10, Color.blue, ComboType.Standart));
            else if (type == BulletTypes.Hellfire)
                _bulletsInRound.Add(new BulletElement(5, 5, Color.red, ComboType.Standart));
            else if (type == BulletTypes.Blood)
                _bulletsInRound.Add(new BulletElement(10, 5, Color.white, ComboType.Standart));
        }

        UIManager.Instance.ChangeComboText("No combo");
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
