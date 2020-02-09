using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance;
    [SerializeField] private GameObject bulletSpawnObject;
    [SerializeField] private SpriteRenderer weaponSprite;
    private bool onLeft = true;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        SetPosition();
        SetAngle();        
    }

    public Vector3 GetSpawnPoint() => bulletSpawnObject.transform.position;

    private void SetAngle()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var weaponPos = transform.position;
        weaponPos.y += 0.12f;

        transform.LookAt(mousePos);

        transform.rotation = Quaternion.LookRotation(-(mousePos - weaponPos), Vector3.forward*transform.localScale.x);
        transform.rotation = Quaternion.Euler(0, 0, transform.transform.eulerAngles.z-90);
    }

    private void SetPosition()
    {
        var pos = Input.mousePosition;
        var leftDelt = 0f;
        var rightDelt = 0f;

        if (onLeft)
            leftDelt = 0.2f;
        else
            rightDelt = 0.2f;

        if (pos.x >= Screen.width/(2-leftDelt))
        {
            transform.localPosition = new Vector3(0.244f, -0.1409f);
            transform.localScale = new Vector3(1, -1, 1);
            onLeft = false;
        }
        else if (pos.x < Screen.width /(2+rightDelt))
        {
            transform.localPosition = new Vector3(-0.244f, -0.1409f);
            transform.localScale = new Vector3(-1, -1, 1);

            onLeft = true;
        }

        if (Player.Instance.IsWatchingUp)
            weaponSprite.sortingOrder = 99;
        else
            weaponSprite.sortingOrder = 101;
    }
}