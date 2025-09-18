using UnityEngine;
using UnityEngine.Splines;

public class Spline_Movement : MonoBehaviour
{
    public SplineContainer thePath;
    public float speed;
    private float duration = 0.0f;

    Vector3 oldPos;

    Vector3 lookDir;
    void Start()
    {
        transform.position = GameObject.Find("Spawnpoint").transform.position;
    }

    void Update()
    {
        duration += (speed * Time.deltaTime) * 0.01f;
        transform.position = thePath.EvaluatePosition(thePath.Spline, duration);
        lookDir = oldPos - transform.position;
        transform.forward = -lookDir;
        oldPos = transform.position;
    }
}
