using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
   public event Action OnEnemyDestroyed;

   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Bullet"))
      {
         Destroy(other.gameObject);
         OnEnemyDestroyed?.Invoke();
         Destroy(gameObject);
      }
   }
}
