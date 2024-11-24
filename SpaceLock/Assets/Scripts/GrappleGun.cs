using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    public Camera playerCamera;
    public Transform shootPosition;
    public float maxRotationAngle = 30f;
    public float rotationSpeed = 5f;
    private Quaternion initialRotation;
    private bool isGrappling = false;
    private Vector3 grapplePoint;

    void Start()
    {
        initialRotation = playerCamera.transform.rotation;
    }

    void Update()
    {
        if (isGrappling)
        {
            UpdateGunRotation();
        }
        else
        {
            ReturnToInitialRotation();
        }
        initialRotation = playerCamera.transform.rotation;
    }

    public void StartGrapple(Vector3 targetPoint)
    {
        isGrappling = true;
        grapplePoint = targetPoint;
    }

    public void StopGrapple()
    {
        isGrappling = false;
    }

    void UpdateGunRotation()
    {
        if (isGrappling)
        {
            Vector3 directionToTarget = grapplePoint - shootPosition.position;

            // Calculate rotation only for X and Y axes
            float rotationX = Mathf.Clamp(Mathf.Atan2(directionToTarget.y, directionToTarget.z) * Mathf.Rad2Deg, -maxRotationAngle, maxRotationAngle);
            float rotationY = Mathf.Clamp(Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg, -maxRotationAngle, maxRotationAngle);

            Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

            // Smoothly rotate towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Return to initial rotation when not grappling
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void ReturnToInitialRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
    }
}
