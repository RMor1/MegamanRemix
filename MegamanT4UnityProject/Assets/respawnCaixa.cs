using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnCaixa : MonoBehaviour
{
    GameObject Respawn;
    [SerializeField]
    private bool respawn;
    // Start is called before the first frame update
    void Start()
    {
        Respawn = GameObject.Find("BarrelSpawner");
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "deSpawn")
        {
            Instantiate(gameObject, Respawn.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
