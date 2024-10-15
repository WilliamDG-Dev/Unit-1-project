using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Helper : MonoBehaviour
{
    LayerMask groundLayerMask;
 
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }
    public bool RayGroundCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;
        bool hitSomething = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + offset, -Vector2.up, rayLength, groundLayerMask);

        Color hitColor = Color.red;

        if (hit.collider != null)
        {
            hitColor = Color.green;
            hitSomething = true;
        }

        Debug.DrawRay(transform.position + offset, -Vector3.up * rayLength, hitColor);

        return hitSomething;
    }

}
