using System;
using UnityEngine;

public class EnemyScriptTest : MonoBehaviour
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
