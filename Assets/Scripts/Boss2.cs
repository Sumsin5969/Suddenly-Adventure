using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 보스 Default 기능
public class boss2 : MonoBehaviour
{
    public Transform player;
    public bool isFlip = false;

    public float enemyhealth;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Rigidbody2D rigid;

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

    public void EnemyHit(int damage)
    {
        enemyhealth -= damage;

        if (enemyhealth <= 0) // 적 사망
        {
            //nextMove = 0;
        
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            spriteRenderer.flipY = true;
            boxCollider.enabled = false;
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
        else // 적 피격
        {
            Debug.Log("보스 적중");           
        }
    }

}
