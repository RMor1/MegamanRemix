using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickTest : MonoBehaviour
{
    [SerializeField] private int nextLevel;
    private GameObject levelLoader;
    private void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            levelLoader.GetComponent<LevelLoader>().LoadLevel(nextLevel);
        }
    }
}
