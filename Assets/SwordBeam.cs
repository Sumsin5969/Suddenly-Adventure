using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SwordBeam : MonoBehaviour
{
    public float beamSpeed; 
    public float beamDistance;
    public LayerMask isLayer;

    void Start()
    {
        Invoke("Destroy", 2);
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, beamDistance, isLayer);

        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                Debug.Log("검기 적중");
            }
            Destroy();
        }

        if (transform.rotation.y == 0)
        {
            transform.Translate(transform.right * beamSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right * -1 * beamSpeed * Time.deltaTime);
        }
        
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
