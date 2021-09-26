using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxAranhaTeto : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("1");
        Debug.Log("2");
        transform.parent.GetComponent<AiAranhaTeto>().vida--;
        Debug.Log(transform.parent.GetComponent<AiAranhaTeto>());
        if (transform.parent.GetComponent<AiAranhaTeto>().vida <= 0) Destroy(transform.parent.parent.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("a");
    }
}
