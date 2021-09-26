using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    [SerializeField] private int nextScene;
    private GameObject levelLoader;
    private bool playerTrigger;
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");
    }
    private void Update()
    {
        if(playerTrigger && Input.GetKey(KeyCode.W)) levelLoader.GetComponent<LevelLoader>().LoadLevel(nextScene);
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        playerTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerTrigger = false;
    }
}
