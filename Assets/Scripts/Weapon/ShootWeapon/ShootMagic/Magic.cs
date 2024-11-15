using Character.Player;
using UnityEngine;

public class Magic : MonoBehaviour
{
   public float speed;
   public float damage;

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
      if (collision.CompareTag("Enemy"))
      {
         collision.gameObject.GetComponent<Character.Character>().TakeDamage(damage);
      }
     
      // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
      GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
      exp.transform.position = transform.position;
      exp.transform.rotation = Quaternion.identity;
      
      // Destroy(this.gameObject);
      ObjectPool.Instance.PushObject(this.gameObject);
   }
}
