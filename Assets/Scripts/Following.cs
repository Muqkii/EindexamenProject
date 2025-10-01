using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Following : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;

    private bool closeToPlayer = true;
    private bool isShooting;

    void Update()
    {
        FakeShooting();
        if (closeToPlayer && !isShooting)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        //pogo mode
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }

    void FakeShooting()
    {
        if(Input.GetMouseButton(0))
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            closeToPlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            closeToPlayer = true;
        }
    }
}
