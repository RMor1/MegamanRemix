using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleSlow : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    // Update is called once per frame
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<ControlV2>().isSlowed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<ControlV2>().isSlowed = false;
        }
 
    }
}

