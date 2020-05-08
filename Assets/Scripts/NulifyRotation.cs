using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulifyRotation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;        
    }
}
