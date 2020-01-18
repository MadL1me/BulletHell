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
    [SerializeField] public Image[] bulletPlaces;
    [SerializeField] public Image roulette;
    [SerializeField] private Image[] bulletsPlaces;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

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