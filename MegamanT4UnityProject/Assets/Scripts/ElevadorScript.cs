using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevadorScript : MonoBehaviour
{
    private bool bossCheckCooldown=true;
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
        if(geradoresBool[0] && geradoresBool[1] && geradoresBool[2])
        {
            GameObject.Find("RigController").GetComponent<BossScript>().leaveHole = true;
        }
        GetComponent<BoxCollider2D>().enabled = false;
    }
    private void Update()
    {
        if(bossCheckCooldown)
        {
            if(GameObject.Find("RigController") == null)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            StartCoroutine(bossCheck());
        }
    }
    IEnumerator bossCheck()
    {
        bossCheckCooldown = false;
        yield return new WaitForSeconds(0.1f);
        bossCheckCooldown = true;
    }
    public void litElevatorLight(int lightNumber)
    {
        luzes[lightNumber].sprite = lit;
    }
}
