using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Following : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    public Vector3 maxRange;

    private Vector3 newPosition;
    private float newX;
    private float newY;
    private float newZ;

    void Start()
    {
        newX = transform.position.x + maxRange.x;
        newY = transform.position.y + maxRange.y;
        newZ = transform.position.z + maxRange.z;
    }

    void Update()
    {
        newPosition = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (newPosition.x >= newX && newPosition.y >= newY && newPosition.z <= newZ)
        {
            transform.position = newPosition;
        }
    }
}
