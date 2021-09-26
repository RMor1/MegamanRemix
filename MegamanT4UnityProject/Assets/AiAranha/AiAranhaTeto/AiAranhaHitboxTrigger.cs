using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAranhaHitboxTrigger : MonoBehaviour
{
    private AiAranhaTeto aiAranhaTeto;
    void Start()
    {
        aiAranhaTeto = gameObject.transform.parent.GetChild(1).GetComponent<AiAranhaTeto>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) aiAranhaTeto.playerDetected = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) aiAranhaTeto.playerDetected = false;
    }
}
