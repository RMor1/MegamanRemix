using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShot : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        GameObject.Find("RigController").GetComponent<ControlRigV3>().vida--;
    }
}
