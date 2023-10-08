using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMP_Text scoreText;

    public float score=0;

    private void Start()
    {
        if (scoreText)
        {
            scoreText.text += score;

        }

    }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void IncrementScore(float val)
    {
        // TODO Increment score logic and win condition 
        score += val;
        scoreText.text += score;


    }
}
