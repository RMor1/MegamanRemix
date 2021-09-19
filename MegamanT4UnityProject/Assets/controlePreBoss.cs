using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class controlePreBoss : MonoBehaviour
{
    public GameObject Player1;
    public GameObject diretorPreBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayableDirector pd = diretorPreBoss.GetComponent<PlayableDirector>();
        if (pd != null)
            pd.Play();
    }
}
