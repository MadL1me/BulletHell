using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SmartCamera : MonoBehaviour
{
    public static SmartCamera Instance;
    private Camera camera;
    private PixelPerfectCamera pixelPerfect;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        GetPosition();
    }

    private void GetPosition()
    {
        var a = 0.35f;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = new Vector3();
        pos.x = (Player.Instance.transform.position.x + a*mousePos.x) / (1 + a);
        a += 0.05f;
        pos.y = (Player.Instance.transform.position.y + a * mousePos.y) / (1 + a);
        pos.z = -10;
        var pos2 = pos - transform.position;
        transform.position += pos2/20;
    }

    private IEnumerator MoveCamera(int value, int speed)
    {

            yield break;
    }

    public IEnumerator ChangeSize(int value, int speed)
    {
        var iterations = 100 / speed;
        var deltaval = (value - pixelPerfect.assetsPPU) / iterations;
        for (float i = 0; i<100; i+=speed)
        {
            pixelPerfect.assetsPPU += deltaval;
            yield return new WaitForFixedUpdate();
        }
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        pixelPerfect = GetComponent<PixelPerfectCamera>();
        //camera.orthographicSize = 10;
    }
}