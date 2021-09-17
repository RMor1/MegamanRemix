using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private string tagPlayer;
    [SerializeField] private string tagGround;
    [SerializeField] private float duration;
    private float timer=0;
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer>duration)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(tagGround))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag(tagPlayer))
        {
            collision.gameObject.GetComponent<ControlV2>().vida--;
            Destroy(gameObject);
        }
    }
}
