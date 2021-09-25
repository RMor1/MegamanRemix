using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsFunctions : MonoBehaviour
{
    private LevelLoader levelLoader;
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
    public void loadingFunctions(string tipo)
    {
        if (tipo == "start")
        {
            levelLoader.finishedStart = true;
        }
    }
}
