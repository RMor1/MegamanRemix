using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleNpcHub : MonoBehaviour
{
    [SerializeField]
    int controle = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(controle)
        {
            case 0:
                GetComponent<Animator>().SetBool("medo1", true);
                break;
            case 1:
                GetComponent<Animator>().SetBool("medo2", true);
                break;
            case 2:
                GetComponent<Animator>().SetBool("medo3", true);
                break;
            case 3:
                GetComponent<Animator>().SetBool("medo4", true);
                break;
        }
    }
}
