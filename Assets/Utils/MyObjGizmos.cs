using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObjGizmos : MonoBehaviour
{
    [Header("Circle")]
    [SerializeField] private bool drawCircle = true;
    [SerializeField] private float circleSize = 1f;

    [Header("Cube")]
    [SerializeField] private bool drawCube = true;
    [SerializeField] private float cubeSize = 1f;

    private void OnDrawGizmos()
    {
        if (drawCube)
            Gizmos.DrawWireCube(transform.position, Vector3.one * cubeSize);
        if (drawCircle)
            Gizmos.DrawWireSphere(transform.position, circleSize);
    }
}
