using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool Instance;
    
    public List<Projectile> bulletsPool { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        bulletsPool = new List<Projectile>();
    }

    public void AddBullet()
    {

    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
