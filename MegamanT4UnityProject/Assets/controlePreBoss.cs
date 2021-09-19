using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class controlePreBoss : MonoBehaviour
{
    private GameObject Player1;
    private GameObject diretorPreBoss;
    private void Start()
    {
        Player1 = GameObject.Find("Player1");
        diretorPreBoss = GameObject.Find("diretorPreBoss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayableDirector pd = diretorPreBoss.GetComponent<PlayableDirector>();
        if (pd != null)
            pd.Play();
    }
}
