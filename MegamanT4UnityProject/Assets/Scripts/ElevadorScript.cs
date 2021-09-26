using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevadorScript : MonoBehaviour
{
    public static bool[] geradoresBool = new bool[3];
    private SpriteRenderer[] luzes = new SpriteRenderer[3];
    [Header("Luzes")]
    [SerializeField] private Sprite lit;
    void Start()
    {
        for(int i=0;i<=2;i++)
        {
            luzes[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if(geradoresBool[i]) luzes[i].sprite = lit;
        }
    }
    public void litElevatorLight(int lightNumber)
    {
        luzes[lightNumber].sprite = lit;
    }
}
