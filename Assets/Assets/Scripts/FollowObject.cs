using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject anchor;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {

        this.transform.position = anchor.transform.position + offset;
    }
}
