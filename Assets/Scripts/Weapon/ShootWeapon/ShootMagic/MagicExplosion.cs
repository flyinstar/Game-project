using UnityEngine;

public class MagicExplosion : MonoBehaviour
{
    private float timer = 2;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // Destroy(this.gameObject);
            ObjectPool.Instance.PushObject(this.gameObject);
            
            timer = 2;
        }
    }
}
