using Unity;
using UnityEngine;
using System;

public enum ComboType
{
    Standart,
    Ricoshet,
    Double,
    Shotgun,
    Demonic,
    Vampire,
    Ghost
}


public class BulletElement
{
    public float Damage;
    public float Speed;
    public float Size;
    public Color bulletColor;
    public ComboType type;
    
    public BulletElement(float dmg, float spd, Color clr, ComboType type, float size = 1)
    {
        Damage = dmg;
        Speed = spd;
        bulletColor = clr;
        this.type = type;
        Size = size;
        bulletColor = Color.red;
    }
}