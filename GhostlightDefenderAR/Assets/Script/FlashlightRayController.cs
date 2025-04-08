using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FlashlightRayController : MonoBehaviour
{
    [Header("Ray Settings")]
    public Camera arCamera; 
    public float rayLength = 10f;

    [Header("Raycast Manager")]
    public ARRaycastManager raycastManager;  
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    private bool isTouching = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            isTouching = true;
            Vector3 worldPoint = GetWorldPoint(Input.mousePosition);
            ShootRay(worldPoint);
        }
        else
        {
            isTouching = false;
        }
#else
        if (Input.touchCount > 0)
        {
            isTouching = true;
            Vector3 worldPoint = GetWorldPoint(Input.GetTouch(0).position);
            ShootRay(worldPoint);
        }
        else
        {
            isTouching = false;
        }
#endif
    }

    Vector3 GetWorldPoint(Vector3 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    void ShootRay(Vector3 targetPoint)
    {
        Debug.DrawRay(arCamera.transform.position, targetPoint - arCamera.transform.position, Color.red);

        RaycastHit hit;
        Ray ray = new Ray(arCamera.transform.position, (targetPoint - arCamera.transform.position).normalized);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.Log("Hit: " + hit.collider.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("Enemy Destroyed!");
            }
        }
    }
}