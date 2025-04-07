using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private Transform player;
    private HealthManager healthManager;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            healthManager = playerObj.GetComponent<HealthManager>();
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure your player has the tag 'Player'");
        }
    }

    void Update()
    {
        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthManager.takeDamage();
            Destroy(gameObject);
        }
    }
}
