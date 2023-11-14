using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public AudioSource audioSource;
    public AudioClip audioBoar;
    public AudioClip audioBoarDie;
    public int enemyhealth;
    public float speed = 2;
    bool isLeft = true;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void EnemyHit(int damage, Vector2 targetPos)
    {
        enemyhealth -= damage;

        if(enemyhealth <= 0) // 적 사망
        {
            //nextMove = 0;
            speed = 0;
            audioSource.clip = audioBoarDie;
            audioSource.Play();
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            spriteRenderer.flipY = true;
            boxCollider.enabled = false;
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
        else // 적 피격
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Transform playerTransform = playerObject.transform;
            anim.SetTrigger("OnDamaged");
            audioSource.clip = audioBoar;
            audioSource.Play();
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            int add = transform.position.x - playerTransform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(add, 0.5f) * 2, ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isLeft = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnemyHit(1, collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Endpoint")
        { 
            if (isLeft)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isLeft = true;
            }
        }   
    }
}