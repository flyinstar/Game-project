using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
   public float speed;

   public GameObject explosionPrefab;
   
   private Rigidbody2D rb;

   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
   }

   public void SetSpeed(Vector2 direction)
   {
      rb.velocity = direction * speed;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
      GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
      exp.transform.position = transform.position;
      
      // Destroy(this.gameObject);
      ObjectPool.Instance.PushObject(this.gameObject);
   }
}
