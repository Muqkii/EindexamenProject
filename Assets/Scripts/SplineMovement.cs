/*using UnityEngine;
using UnityEngine.Splines;

public class Spline_Movement : MonoBehaviour
{
    public SplineContainer thePath;
    public float speed;
    private float duration = 0.0f;

    Vector3 oldPos;
    Vector3 startPos;
    Vector3 lookDir;
    void Start()
    {
        startPos = thePath.EvaluatePosition(thePath.Spline, 0.0f);
    }

    void Update()
    {
        duration += (speed * Time.deltaTime) * 0.01f;
        transform.position = thePath.EvaluatePosition(thePath.Spline, duration);
        lookDir = oldPos - transform.position;
        transform.forward = -lookDir;
        oldPos = transform.position;
    }
}*/

using UnityEngine;
using UnityEngine.Splines;

public class Spline_Movement : MonoBehaviour
{
    public SplineContainer thePath;
    public float speed;
    public bool loop = true; // Toggle for looping behavior

    private float duration = 0.0f;
    Vector3 oldPos;
    Vector3 startPos;
    Vector3 lookDir;

    void Start()
    {
        startPos = thePath.EvaluatePosition(thePath.Spline, 0.0f);
        oldPos = startPos; // Initialize oldPos to avoid initial jump
    }

    void Update()
    {
        duration += (speed * Time.deltaTime) * 0.01f;

        // Handle looping
        if (loop)
        {
            // Wrap duration between 0 and 1 for continuous looping
            duration = duration % 1.0f;
        }
        else
        {
            // Clamp duration between 0 and 1 for one-time movement
            duration = Mathf.Clamp01(duration);
        }

        transform.position = thePath.EvaluatePosition(thePath.Spline, duration);

        // Calculate look direction
        lookDir = oldPos - transform.position;

        // Only update rotation if there's actual movement
        if (lookDir.magnitude > 0.001f)
        {
            transform.forward = -lookDir;
        }

        oldPos = transform.position;
    }
}
