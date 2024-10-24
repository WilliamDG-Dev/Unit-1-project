using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Camera following player at all times
public class CameraFollow : MonoBehaviour
{
    Helper helper;
    public GameObject player;
    private Vector3 offset = new Vector3(0, -1, -9);
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
