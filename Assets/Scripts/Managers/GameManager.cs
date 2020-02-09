using Unity;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isPause { get; set; }

    private void Start()
    {
        //isPause = true;
        Instance = this;
        if (PlayerInfo.IsPLayedBefore == false)
        {
            StartCoroutine(TrainingManager.Instance.StartLearning());
        }
    }
}