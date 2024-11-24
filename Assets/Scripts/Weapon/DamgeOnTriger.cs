using UnityEngine;

public class DamgeOnTriger : MonoBehaviour
{
    public float damage = 1f;
    
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Character.Character>().TakeDamage(damage);
    }
}
