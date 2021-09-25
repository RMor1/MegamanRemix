using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleSpawn : MonoBehaviour
{
    bool spawnar = true;
    [SerializeField]
    float intervaloSpawn = 2;
    [SerializeField]
    GameObject prefabPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (spawnar)
        {
            Vector3 posSpawn = transform.position;
            GameObject tempPlataform = Instantiate(prefabPlatform,
            posSpawn,
            Quaternion.identity, null);
            StartCoroutine("frequenciaSpawn");
        }
    }
    IEnumerator frequenciaSpawn()
    {
        spawnar = false;
        yield return new WaitForSeconds(intervaloSpawn);
        spawnar = true;
    }
}
