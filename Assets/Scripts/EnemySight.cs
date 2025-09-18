using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    public float DetectionRange;
    public float DetectionAngle;

    public Vector3 Offset;

    bool isInAngle;
    bool isInRange;
    bool isNotHidden;

    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;

        if (Vector3.Distance(transform.position, Player.transform.position) < DetectionRange)
        {
            isInRange = true;
            //Debug.Log("player is in range");
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.transform.position - transform.position + Offset), out hit, Mathf.Infinity))
        {
            if (hit.transform == Player.transform)
            {
                isNotHidden = true;
                //Debug.Log("player is not hidden");
            }
        }

        Vector3 side1 = Player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if (angle < DetectionAngle && angle < -1 * DetectionAngle)
        {
            isInAngle = true;
            //Debug.Log("player is in fov");

        }

        if (isInRange == true && isNotHidden == true && isInAngle == true)
        {
            //Debug.Log("player is detected");
        }
    }
}
