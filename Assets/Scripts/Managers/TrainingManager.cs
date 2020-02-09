using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public static TrainingManager Instance;

    public Collider2D[] Colliders;

    void Start()
    {
        Instance = this;
    }

    public IEnumerator StartLearning()
    {
        //Prepare player to start

        StartCoroutine(UIManager.Instance.IntroToGame());
        yield return new WaitForFixedUpdate();
    }

    public void Introduce() => StartCoroutine(UIManager.Instance.Introduce());

    public void LearnToCrafts() => StartCoroutine(UIManager.Instance.LearnToCrafts());

    public void LearnToShoot() => StartCoroutine(UIManager.Instance.LearnToShoot());

    public void LearnToDash() => StartCoroutine(UIManager.Instance.LearnToDash());
}