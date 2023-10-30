using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;  
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    bool isMoving = false;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector3 boxSize;
    public GameManager gameManager;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        // �������� ����
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // ���� ��ȯ
        if(Input.GetButton("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        float h = Input.GetAxis("Horizontal"); // Ű���� �Է°�

        
            
        // Ű �Է��� �ִ� ��쿡�� �ִϸ��̼��� ����
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // �׻� �ִ� �ӵ��� ����
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // ���� �ִϸ��̼�
        anim.SetBool("isWalking", isMoving);

        if (curTime <= 0)
        {
            // 'Z' Ű�� ����� ����
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                // ���� ���� �ȿ��� �����ϸ� ����� �α� ���
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                }

                anim.SetTrigger("attack");
                curTime = coolTime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    // ���� ���� �׸���
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    void FixedUpdate()
    {
        if(rigid.velocity.y < 0) // ������ ���� ����ĳ��Ʈ�� ��
        {
            // ����ĳ��Ʈ �׸���
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // ����ĳ��Ʈ ��Ʈ, ���̾��ũ �ν�
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // ����
            if(rayHit.collider != null)
                if(rayHit.distance < 1.2f)
                    anim.SetBool("isJumping", false);
                    //Debug.Log(rayHit.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // �ǰ�
    {
        // �÷��̾ ���Ͷ� ���˽�
        /*
         최적화가 하고싶으면 gameObject.CompareTag("태그이름")) <<<< 이 CompareTag 함수를 사용해야돼.
         자세한게 궁금하면 구글에 검색하면 나와
         */
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point ȹ��
            /* 나중에 아이템 많아지면 어쩌려고..? 아이템 하나당 변수1 조건문1 추가해야되네
                이런 기능은 "Item" 이라는 클래스를 하나 만들어 상속시키는 방법으로 쉽게 구현 가능해.
                Item 클래스는 그냥 상속시키는게 아니라 abstract 클래스, 추상 클래스로 만들어.
                override 해야할 함수로써 GetItem 이런식으로 함수를 Item클래스에 추상 표현을 해놓는거지.
                그 다음 Item 클래스를 상속받는 Bronze Silver Gold 클래스를 만드는거지. 
                Bronze Silver Gold 클래스를 추상클래스를 상속받았기 때문에 반드시 부모 클래스의 추상 함수를 구현해야해.
                거기서 각각 함수에 Bronze 의 GetItem 에는 +50점 silver는 +100점 이런식으로 만들어놓는거야.
                그러고 Item 태그인 오브젝트를
                Item item =  collision.GetComponent<Item>(); <<< 이런식으로 Bronze Silver Gold 클래스를 가져올수있어. Item 클래스를 상속받았기때문에 가능해.
                그다음 item.GetItem(); 이렇게 함수를 호출하면 해당 아이템이 뭔지는 구별 할 필요없이 해당 아이템의 획득 효과를 따로따로 구현할수있는거야.
            */


            // 이건 내가 예시로 만들어준거야
            Item item = collision.GetComponent<Item>();
            item.GetItem();
            // 이런식으로 구현하기 위해선 GameManager가 가지고있는 스테이지 점수를 접근할 방법이 없잖아?
            // 그래서 GameManager를 싱글톤으로 구현해서 다른 클래스에서도 언제든지 접근할수있게 하는 방법이 좋아.
            // 이런식으로 구현 해놓으면 나중에 아이템이 새로 생기더라도 이 함수는 건드릴 필요없이 딱 2줄로 끝나지?
            // 새로운 아이템이 생길때마다 Item 클래스를 상속시켜서 따로따로 기능을 구현하기만 하면 되는거지.
            // 코드가 길어지지도 않고 가독성도 좋고 유지보수도 좋은 방법이니까 꼭 알고가면 좋겠어.

            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if(isBronze)
                gameManager.stagePoint += 50;
            else if(isSilver)
                gameManager.stagePoint += 100;
            else if(isGold)
                gameManager.stagePoint += 300;
            // Item ����
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }
    private void OnDamaged(Vector2 targetPos) // �ǰݽ� ����
    {
        // ü�� ����
        // 나중에 다른 몬스터가 추가된다고 치면 몬스터마다 받는 대미지가 달라야 할거 같지않아? 
        // 대미지를 준다면 몬스터의 스탯에 비례하도록 만들면 추상적 표현으로 바로 구현될텐데.
        // 이런식으로 매니저 함수에 의존하면 나중가서 몬스터 테그 하나하나씩 비교해서 대미지 다르게 줄거잖아.
        gameManager.HealthDown();

        // ���̾ PlayDameged�� ����
        gameObject.layer = 11;

        // �ǰݽ� ���� �� ������ ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // �ǰݽ� �з���
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // �ִϸ��̼�
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1); // Player ���̾�� ���ư��� �ð�(�����ð�����)
    }
    private void OffDamaged() // ���̾ Player�� ����
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie() // �÷��̾� �����
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
