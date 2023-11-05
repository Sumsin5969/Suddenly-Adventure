using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public AudioSource audioSource;
    public AudioClip audioBoar;
    public AudioClip audioBoarDie;
    public int nextMove;
    public int enemyhealth;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        Invoke("Think", 3);
    }


    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
            Turn();
    }

    // 재귀 함수
    void Think()
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2);

        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // Recursive(재귀)
        float nextThinkTime = Random.Range(1f, 2f);
        Invoke("Think", nextThinkTime);
    }
    
    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 3);
    }

    public void EnemyHit(int damage, Vector2 targetPos2)
    {
        enemyhealth -= damage;

        if(enemyhealth <= 0) // 적 사망
        {
            Invoke("Think", 0);
            audioSource.clip = audioBoarDie;
            audioSource.Play();
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            spriteRenderer.flipY = true;
            boxCollider.enabled = false;
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
        else // 적 피격
        {
            anim.SetTrigger("doDamagedBoar");
            audioSource.clip = audioBoar;
            audioSource.Play();
            int dirc = transform.position.x - targetPos2.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 0.5f) * 5, ForceMode2D.Impulse);
                    
        }
    }
}