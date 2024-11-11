using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicExplosion : MonoBehaviour
{
    private float timer = 2;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
