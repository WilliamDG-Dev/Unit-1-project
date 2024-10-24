using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// consumables respawning after period of time
public class ConsumableRespawn : MonoBehaviour
{
    public GameObject consumable;
    private float timer = 0;
    void Update()
    {
        if (!consumable.activeSelf)
        {
            Debug.Log("consumed");
            int respawn = Random.Range(10,21);
            Wait(respawn);
        }
    }
    void Wait(float wait)
    {
        timer += Time.deltaTime;
        if (timer >= wait)
        {
            consumable.gameObject.SetActive(true);
            timer = 0;
        }
    }
}
