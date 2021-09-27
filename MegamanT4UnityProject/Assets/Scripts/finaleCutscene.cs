using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class finaleCutscene : MonoBehaviour
{
    private bool triggered;
    private bool onEndScreen;
    private void Update()
    {
        if(triggered && Input.GetKeyDown(KeyCode.W) && onEndScreen==false)
        {
            GameObject loading = GameObject.Find("LoadingScreen");
            onEndScreen = true;
            loading.transform.GetChild(0).GetComponent<Text>().text = "FIM";
            loading.GetComponent<Animator>().SetTrigger("loadingstart");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
    }
}
