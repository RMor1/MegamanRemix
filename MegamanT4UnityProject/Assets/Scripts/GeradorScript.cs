using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorScript : MonoBehaviour
{
    [Range(1, 3),SerializeField] private int Gerador;
    private bool on;
    private bool outlineState=true;
    private bool playerTrigger;
    [Header("Gerador Luzes")]
    [SerializeField] private Sprite lit;
    void Start()
    {
    }
    private void Update()
    {
        if (playerTrigger && Input.GetKey(KeyCode.W) && on==false)
        {
            turnOnLights();
            on = true;
            turnOffOutline();
            if (Gerador == 2)
            {
                ElevadorScript.geradoresBool[Gerador-1] = true;
                GameObject.Find("Elevador").GetComponent<ElevadorScript>().litElevatorLight(1);
            }
            else ElevadorScript.geradoresBool[Gerador-1] = true;
        }
        else if(on==false && outlineState==true && ElevadorScript.geradoresBool[Gerador-1])
        {
            turnOnLights();
            turnOffOutline();
        }
    }
    private void turnOnLights()
    {
        for (int i = 0; i < 2; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = lit;
        }
    }
    private void turnOffOutline()
    {
        GetComponent<BoxCollider2D>().enabled=false;
        outlineState = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerTrigger = false;
    }
}
