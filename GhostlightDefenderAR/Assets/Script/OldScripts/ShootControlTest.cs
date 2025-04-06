using UnityEngine;

public class ShootControlTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraPosition;
    
    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 3f;
    
    void Start()
    {
        UIManagerTest.OnShootButtonPressed += Shoot;
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, cameraPosition.position, Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraPosition.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, bulletLifeTime);
    }
}
