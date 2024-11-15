using UnityEngine;

public class MagicWeapon : MonoBehaviour
{
    private static readonly int Fire1 = Animator.StringToHash("Fire");
    public float interval = 0.5f;
    
    public GameObject magicPrefab;
    
    private Transform topPos;
    
    private Vector2 mousePos;
    
    private Vector2 direction;

    private float timer;
    
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        topPos = transform.Find("Top");
    }

    private void Update()
    {
        if (Camera.main != null)
            mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition +
                                                      new Vector3(0, 0, Camera.main.transform.position.z));
        Shoot();
    }

    private void Shoot()
    {
        direction = new Vector2(transform.position.x, transform.position.y) - mousePos;
        transform.right = direction.normalized;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        if (UnityEngine.Input.GetButtonDown("Fire1"))
        {
            if (timer == 0)
            {
                Fire();
                timer = interval;
            }
        }
    }

    void Fire()
    {
        animator.SetTrigger(Fire1);
        
        // GameObject magic = Instantiate(magicPrefab, staffTopPos.position, Quaternion.identity);
        GameObject magic = ObjectPool.Instance.GetObject(magicPrefab);
        magic.transform.position = topPos.position;
        magic.transform.rotation = Quaternion.identity;
        
        magic.GetComponent<Magic>().SetSpeed(direction);
    }
}
