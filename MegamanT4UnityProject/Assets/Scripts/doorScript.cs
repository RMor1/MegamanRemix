using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    [SerializeField] private int nextScene;
    [SerializeField] private bool lockUnlockSystem;
    private GameObject levelLoader;
    private bool playerTrigger;
    private bool locked,foreverLocked;
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");
        if(nextScene==2 && ElevadorScript.geradoresBool[0])
        {
            foreverLocked = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (nextScene == 3 && ElevadorScript.geradoresBool[2])
        {
            foreverLocked = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (lockUnlockSystem)
        {
            locked = true;
            GetComponent<BoxCollider2D>().enabled = false;
            if (ElevadorScript.geradoresBool[1])
            {
                transform.GetChild(0).position = new Vector3(transform.position.x - 1.5f, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
                GetComponent<BoxCollider2D>().enabled = true;
                locked = false;
            }
        }
    }
    private void Update()
    {
        if(!foreverLocked)
        {
            if (locked)
            {
                if (ElevadorScript.geradoresBool[1])
                {
                    transform.GetChild(0).position = new Vector3(transform.position.x - 1.5f, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
                    GetComponent<BoxCollider2D>().enabled = true;
                    locked = false;
                }
            }
            else if (playerTrigger && Input.GetKey(KeyCode.W)) levelLoader.GetComponent<LevelLoader>().LoadLevel(nextScene);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerTrigger = false;
    }
}
