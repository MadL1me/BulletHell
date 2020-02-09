using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLantern : MonoBehaviour
{
    private Light2D light2D;

    [SerializeField] private float defIntensity;
    [SerializeField] private float deltaIntensity;

    [SerializeField] private float defOR;
    [SerializeField] private float deltaOR;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        RotateToMouse();
        FlickeringEffect();
    }


    private void RotateToMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var laternPos = transform.position;

        transform.LookAt(mousePos);
        transform.rotation = Quaternion.LookRotation(-(mousePos - laternPos), Vector3.forward * transform.localScale.x);
        transform.rotation = Quaternion.Euler(0, 0, transform.transform.eulerAngles.z);
    }

    private void FlickeringEffect()
    {
        light2D.intensity= Random.Range(defIntensity - deltaIntensity, 1);
        light2D.pointLightInnerRadius = Random.Range(defOR - deltaOR, defOR + deltaOR);
    }
}
