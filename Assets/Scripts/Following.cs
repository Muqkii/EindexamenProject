using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Following : MonoBehaviour
{
    public GameObject player;
    public GameObject CageDoor;
    public float speed = 1f;
    public bool closeToPlayer;

    private bool isShooting;
    public bool cageDoorOpen = false;

    void Update()
    {
        cageDoorOpen = CageDoor.GetComponentInChildren<CageDoorScript>().doorOpen; //check if cage is open
        FakeShooting();
        if (!closeToPlayer && !isShooting && cageDoorOpen)
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
            closeToPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            closeToPlayer = false;
        }
    }
}
