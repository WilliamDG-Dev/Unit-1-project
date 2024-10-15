using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Helper helper;
    public GameObject player;
    private Vector3 offset = new Vector3(0, 3, -9);
    private bool borderLeft;
    private bool borderRight;

    void Start()
    {
        helper = gameObject.AddComponent<Helper>();
    }
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
