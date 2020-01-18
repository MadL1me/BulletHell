using Unity;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [SerializeField] public Image[] playerHP;
    [SerializeField] public GameObject roulette;
    [SerializeField] public Image[] bulletsPlaces;

    [SerializeField] private Sprite stdBullet;
    [SerializeField] private Sprite hellBullet;
    [SerializeField] private Sprite silverBullet;
    [SerializeField] private Sprite recoshetBullet;

    public float rotatingSpeed = 1; 

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public IEnumerator InitBossBar()
    {
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator RotateRevolverRoll(bool toDefaultRotation = false)
    {
        var delta = 72 / (100 / rotatingSpeed);
        if (toDefaultRotation)
            delta = roulette.GetComponent<RectTransform>().rotation.eulerAngles.z / (100 / rotatingSpeed);


        for (float i = 0; i<100; i+=rotatingSpeed)
        {
            roulette.gameObject.transform.Rotate(new Vector3(0,0,-delta));
            yield return new WaitForFixedUpdate();
        }
    }

    public void AddBullet(BulletTypes bulletTypes, List<BulletTypes> types)
    {
        var col = new Color();
        if (bulletTypes == BulletTypes.Standart)
            col = Color.white;
        else if (bulletTypes == BulletTypes.Silver)
            col = Color.blue;
        if (bulletTypes == BulletTypes.Recoshet)
            col = Color.green;
        if (bulletTypes == BulletTypes.Hellfire)
            col = Color.red;
        StartCoroutine(RotateRevolverRoll());
        bulletsPlaces[types.Count - 1].color = col;
    }

    public void ShootBullet(List<Bullet> bulletsInRound)
    {
        bulletsPlaces[bulletsInRound.Count].color = Color.black;
    }

    public void ChangeHeartsCount(int value, int capacity)
    {
        if (value > capacity)
            value = capacity;
        for (int i = 0; i < value; i++)
            playerHP[i].color = Color.white;
        for (int i = value; i < capacity; i++)
            playerHP[i].color = Color.black;

    }

    public void ChangeHeartsCapacity(int value)
    {
        for (int i = 0; i < value; i++)
            playerHP[i].enabled = true;
        for (int i = value; i < playerHP.Length; i++)
            playerHP[i].enabled = false;
    }
}