using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public GameObject door;
    public string message;
    public AvailableKeys hotkey;
    private Vector3 startPosition = new Vector3();
    public GameObject openPosition;
    private bool shouldOpen = false;
    private bool inTrigger = false;
    public float movementSpeed;
    public enum AvailableKeys
    {
        E,
        F
    }


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shouldOpen = true;
            }
        }
        if (shouldOpen)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, openPosition.transform.position, movementSpeed);
        }
        else
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, startPosition, movementSpeed);
        }
        Debug.Log(inTrigger);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(message);
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
            shouldOpen = false;
        }
    }
}
