using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
   public float speed;

   public GameObject explosionPrefab;
   
   private Rigidbody2D rb2d;

   private void Awake()
   {
      rb2d = GetComponent<Rigidbody2D>();
   }

   public void SetSpeed(Vector2 direction)
   {
      rb2d.velocity = direction * speed;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      Instantiate(explosionPrefab, transform.position, Quaternion.identity);
      Destroy(gameObject);
   }
}
