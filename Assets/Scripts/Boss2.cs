using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 보스 회전 기능
public class boss2 : MonoBehaviour
{
    public Transform player;
    public bool isFlip = false;
   
    public void LookPlayer()
    {
        Vector3 flip = transform.localScale;
        flip.z *= -1f;
        
        if(transform.position.x > player.position.x && isFlip)
        {
            transform.localScale = flip;
            transform.Rotate(0, 180, 0);
            isFlip = false;
        }
        else if (transform.position.x < player.position.x && !isFlip)
        {
            transform.localScale = flip;
            transform.Rotate(0, 180, 0);
            isFlip = true;
        }
    }

}
