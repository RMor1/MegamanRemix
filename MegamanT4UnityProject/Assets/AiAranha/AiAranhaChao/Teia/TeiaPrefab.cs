using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeiaPrefab : MonoBehaviour
{
    [SerializeField] private float slowDurantion;
    [SerializeField] private float lifeTime;
    [SerializeField] private string playerTag;
    [SerializeField] private string[] collideWithTags;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(playerTag))
        {
            ControlV2 playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlV2>();
            playerControl.Damage();
            if(!playerControl.isSlowed)
            {
                playerControl.slowPlayer(slowDurantion);
            }
            Destroy(gameObject);
        }
    }
}
