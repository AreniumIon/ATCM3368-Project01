using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform objectToFollow = null;

    Vector3 objectOffset;

    private void Awake()
    {
        objectOffset = this.transform.position - objectToFollow.position;
    }

    private void LateUpdate()
    {
        this.transform.position = objectToFollow.position + objectOffset;
    }
}
