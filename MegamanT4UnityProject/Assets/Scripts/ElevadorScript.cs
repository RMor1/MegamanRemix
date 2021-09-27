using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevadorScript : MonoBehaviour
{
    private bool bossCheckCooldown=true;
    private bool onFinalCutscene;
    public bool finishedStartLoading;
    private Animator loadingScreenAnimator;
    public static bool[] geradoresBool = new bool[3];
    private SpriteRenderer[] luzes = new SpriteRenderer[3];
    private bool triggered;
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
        loadingScreenAnimator = GameObject.Find("LoadingScreen").GetComponent<Animator>();
    }
    private void Update()
    {
        if(bossCheckCooldown)
        {
            if(GameObject.Find("RigController") == null)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                bossCheckCooldown = false;
            }
            else StartCoroutine(bossCheck());
        }
        if(triggered && Input.GetKeyDown(KeyCode.W))
        {
            if(!onFinalCutscene)StartCoroutine(finalCutscene());
        }
    }
    IEnumerator finalCutscene()
    {
        loadingScreenAnimator.SetTrigger("loadingstart");
        onFinalCutscene = true;
        while(!GameObject.Find("LevelLoader").GetComponent<LevelLoader>().finishedStart)
        {

            yield return null;
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-11.88f, 44.82f, 0);
        yield return new WaitForSeconds(2);
        loadingScreenAnimator.SetTrigger("loadingend");
        onFinalCutscene = false;
    }
    IEnumerator bossCheck()
    {
        bossCheckCooldown = false;
        yield return new WaitForSeconds(0.1f);
        bossCheckCooldown = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
    }
    public void litElevatorLight(int lightNumber)
    {
        luzes[lightNumber].sprite = lit;
    }
}
