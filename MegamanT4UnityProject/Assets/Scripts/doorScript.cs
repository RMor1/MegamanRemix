using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    [SerializeField] private int nextLevel;
    private GameObject levelLoader;
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");
    }
    void loadLevel()
    {
        levelLoader.GetComponent<LevelLoader>().LoadLevel(nextLevel);
    }

}
