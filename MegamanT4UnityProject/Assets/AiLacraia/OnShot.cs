using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShot : MonoBehaviour
{
    private GameObject rigController;
    [SerializeField] private bool isBoss;
    private void Start()
    {
        rigController = GameObject.Find("RigController");
    }
    private void OnParticleCollision(GameObject other)
    {
        if(isBoss)
        {
            rigController.GetComponent<BossScript>().vida--;
        }
        else
        {
            if (rigController.GetComponent<ControlRigV3>().vida == 3 && rigController.GetComponent<ControlRigV3>().holePos == 0)
            {
                rigController.GetComponent<ControlRigV3>().cutscene = false;
            }
            rigController.GetComponent<ControlRigV3>().vida--;
        }
    }
}
