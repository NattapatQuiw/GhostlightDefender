using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class FlashlightRayController : MonoBehaviour
{
    [Header("Ray Settings")]
    public Camera arCamera;
    public float rayLength = 10f;

    [Header("Raycast Manager")]
    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    [Header("Touch UI")]
    public RectTransform touchEffectUI; // Image ที่ใช้แสดงบนจอ
    public Canvas canvas;               // Canvas ที่ UI อยู่

    private bool isTouching = false;

    void Start()
    {
        if (touchEffectUI != null)
        {
            touchEffectUI.gameObject.SetActive(false); // ซ่อนไว้ก่อน
        }
    }

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
            Vector2 screenPos = Input.mousePosition;

            ShowTouchUI(screenPos);
            Vector3 worldPoint = GetWorldPoint(screenPos);
            ShootRay(worldPoint);
        }
        else
        {
            isTouching = false;
            HideTouchUI();
        }
#else
        if (Input.touchCount > 0)
        {
            isTouching = true;
            Vector2 screenPos = Input.GetTouch(0).position;

            ShowTouchUI(screenPos);
            Vector3 worldPoint = GetWorldPoint(screenPos);
            ShootRay(worldPoint);
        }
        else
        {
            isTouching = false;
            HideTouchUI();
        }
#endif
    }

    Vector3 GetWorldPoint(Vector2 screenPosition)
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
        if (!isTouching) return;

        Ray ray = new Ray(arCamera.transform.position, (targetPoint - arCamera.transform.position).normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("Enemy Destroyed!");
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
        }
    }

    void ShowTouchUI(Vector2 screenPosition)
    {
        if (touchEffectUI == null || canvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        touchEffectUI.anchoredPosition = localPoint;
        touchEffectUI.gameObject.SetActive(true);
    }

    void HideTouchUI()
    {
        if (touchEffectUI != null)
        {
            touchEffectUI.gameObject.SetActive(false);
        }
    }
}
