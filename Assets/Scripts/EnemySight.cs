using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("Detection Settings")]
    public float DetectionRange = 10f;
    public float DetectionAngle = 60f;
    public Vector3 Offset = Vector3.zero;
    public float detectionTime = 2f; 

    [Header("References")]
    public GameObject Player;

    [Header("Look At Settings")]
    public bool enableLookAt = true;
    public float lookAtSpeed = 2f;

    [Header("Gun Settings")]
    public float shootingRange = 20f;
    public float fireRate = 1f;
    public int damage = 10;
    public Transform firePoint;

    [Header("Debug")]
    public bool showDebugRays = true;

    // Detection states
    private bool isInAngle;
    private bool isInRange;
    private bool isNotHidden;
    private bool isSpotted;

    // Detection progress
    private float detectionProgress = 0f;
    private bool isDetecting = false;

    // Enemy Fire
    private Transform playerPos;
    private float nextFireTime;

    void Start()
    {

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerPos = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Geen player gevonden");
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        CheckPlayerDetection();
        UpdateDetectionProgress();
        HandleLookAtPlayer();

        if (isSpotted && CanShoot())
        {
            Shoot();
        }

        if (showDebugRays)
        {
            DrawDebugRays();
        }
    }

    void CheckPlayerDetection()
    {
        // Reset detection flags
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;

        if (Player == null) return;

        // Check range
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= DetectionRange)
        {
            isInRange = true;
            //Debug.Log("Player is in range");
        }

        // Check if player is in field of view
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= DetectionAngle * 0.5f)
        {
            isInAngle = true;
        }

        // Check if player is not hidden (raycast)
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Player.transform.position - rayOrigin + Offset;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection.normalized, out hit, DetectionRange))
        {
            if (hit.transform == Player.transform)
            {
                isNotHidden = true;
            }
        }
    }

    void UpdateDetectionProgress()
    {
        // Check if all conditions are met
        bool canDetect = isInRange && isNotHidden && isInAngle;

        if (canDetect && !isSpotted)
        {
            // Increase detection progress
            isDetecting = true;
            detectionProgress += Time.deltaTime;
            Debug.Log("Player can be spotted");

            // Check if fully detected
            if (detectionProgress >= detectionTime)
            {
                isSpotted = true;
                Debug.Log("Player spotted!");
            }
        }
        else if (!canDetect && (isDetecting || isSpotted))
        {
            // Decrease detection progress when conditions not met
            detectionProgress -= Time.deltaTime * 2f; // Lose detection faster

            if (detectionProgress <= 0f)
            {
                detectionProgress = 0f;
                isDetecting = false;

                if (isSpotted)
                {
                    isSpotted = false;
                    Debug.Log("Player lost!");
                }
            }
        }

        // Clamp progress
        detectionProgress = Mathf.Clamp(detectionProgress, 0f, detectionTime);
    }
    void HandleLookAtPlayer()
    {
        if (!enableLookAt || Player == null || !isSpotted) return;

        // Calculate direction to player
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;

        // Only rotate on Y axis (horizontal rotation)
        directionToPlayer.y = 0;

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
    }
    bool CanShoot()
    {
        return Time.time >= nextFireTime;
    }

    void Shoot()
    {
        nextFireTime = Time.time + (1f / fireRate);

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Player.transform.position - rayOrigin + Offset;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection.normalized, out hit, DetectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                //health playerHealth = hit.transform.GetComponent<health>();
                var playerStats = GameObject.Find("First Person Player").GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("Speler geraakt! Geen PlayerHealth component gevonden.");
                }
            }
        }
    }
    void DrawDebugRays()
    {
        // Draw FOV boundaries
        Vector3 leftBoundary = Quaternion.Euler(0, -DetectionAngle * 0.5f, 0) * transform.forward * DetectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, DetectionAngle * 0.5f, 0) * transform.forward * DetectionRange;

        Debug.DrawRay(transform.position, leftBoundary, Color.blue);
        Debug.DrawRay(transform.position, rightBoundary, Color.blue);
        Debug.DrawRay(transform.position, transform.forward * DetectionRange, Color.green);

        // Draw ray to player
        if (Player != null)
        {
            Color rayColor = (isInRange && isNotHidden && isInAngle) ? Color.red : Color.gray;
            Debug.DrawRay(transform.position + Offset, (Player.transform.position - transform.position - Offset).normalized * DetectionRange, rayColor);
        }
    }
}