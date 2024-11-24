using System;
using UnityEngine;

public class check : MonoBehaviour
{
    public LayerMask layerMask;

    private void Update()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 direction = Camera.main.transform.forward;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);
        if (hit)
        {
            // Debug.DrawRay(origin, direction, Color.red);
            Debug.Log("Hit object: " + hit.collider.gameObject.name + " on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
        }
        else
        {
            // Debug.DrawRay(origin, direction, Color.green);
            Debug.Log("Did not hit any object on the specified layer.");
        }
    }
}