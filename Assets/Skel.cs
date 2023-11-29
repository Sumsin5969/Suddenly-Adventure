using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skel : MonoBehaviour
{
    public Transform player;
    public bool isFlip = false;
    public float moveRange;

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 색상 설정
        Gizmos.DrawWireSphere(transform.position, moveRange);
    }
}
