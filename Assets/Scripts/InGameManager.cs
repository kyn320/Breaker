using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public UIInGame ui;

    public WallBehaviour wall;

    public float playTime, maxPlayTime = 120f, addTime = 30f;

    public bool isPlay = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayGame();
    }

    void Update()
    {
        if (isPlay)
        {
            playTime -= Time.deltaTime;
            ui.timer.SetTime(playTime / 60,  playTime % 60);
            if (playTime <= 0)
            {
                isPlay = false;
                TimeOut();
            }
        }
    }

    public void PlayGame()
    {
        playTime = maxPlayTime;
        isPlay = true;

    }

    public void TimeOut()
    {

    }

    public void EndGame()
    {

    }

}
