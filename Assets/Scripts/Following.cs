using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Following : MonoBehaviour
{
    public GameObject player;
    public GameObject CageDoor;
    public float speed = 1f;

    private bool closeToPlayer = true;
    private bool isShooting;
    private bool cageDoorOpen = false;

    void Update()
    {
        cageDoorOpen = CageDoor.GetComponentInChildren<CageDoorScript>().doorOpen; //check if cage is open
        FakeShooting();
        if (closeToPlayer && !isShooting && cageDoorOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.LookAt(player.transform.position);
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
