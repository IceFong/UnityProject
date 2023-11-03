using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    public Transform orientation;
    void Update()
    {
        transform.rotation = orientation.rotation;     
    }
}
