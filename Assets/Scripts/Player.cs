using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioAttack2;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public AudioClip audioDash;
    public AudioClip audioDashCooldown;
    public float maxSpeed;
    public float speed;
    public float jumpPower;
    public int jumpCnt;
    public int jumpCount;
    public float dashSpeed;
    public float dashTime;
    public float defaultTime;
    public float checkRaidus;
    public LayerMask isLayer;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;
    bool isMoving = false;
    bool isDash = false;
    bool isGround;
    private float curTime;
    public float coolTime = 0.3f;
    public float dashCooltime;
    public float dashFilltime;
    public float comboTime = 0f;
    public float comboResetTime = 0.5f;
    public int comboCount = 0;
    public Transform pos;
    public Transform posJump;
    public Vector3 boxSize;
    public GameObject SwordBeam;
    public GameObject Player;
    public Transform SavePoint;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        jumpCnt = jumpCount;
    }

    void PlaySound(string action) // Sound
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                audioSource.Play();
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                audioSource.Play();
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                audioSource.Play();
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                audioSource.Play();
                break;
            case "DIE":
                audioSource.clip = audioDie;
                audioSource.Play();
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                audioSource.Play();
                break;
        }
    }
    
    
    void Update()
    {
        isGround = Physics2D.OverlapCircle(posJump.position, checkRaidus, isLayer);
        if(rigid.velocity.y > 0.0f)
            anim.SetBool("isJumping", true);
        // 점프 (jumpCount 조절해서 2단 점프 가능)
        if (isGround == true && Input.GetKeyDown(KeyCode.Z) && jumpCnt > 0)
        {
            ResetCombo();
            rigid.velocity = Vector3.up * jumpPower;
            anim.SetBool("isJumping", true);
            PlaySound("JUMP"); // Sound
        }
        if (isGround == false && Input.GetKeyDown(KeyCode.Z) && jumpCnt > 0)
        {
            ResetCombo();
            rigid.velocity = Vector3.up * jumpPower;
            anim.SetBool("isJumping", true);
            PlaySound("JUMP"); // Sound
        }

        if (rigid.velocity.y < 0) // 내려갈 때
        {
            // 레이캐스트 그리기
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            // 레이캐스트 히트, 레이어마스크 인식
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            // 착지
            if(rayHit.collider != null){
                if(rayHit.distance < 1.0f){
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isFalling", false);
                    // Debug.Log(rayHit.collider.name);
                }
            }
            else
                anim.SetBool("isFalling", true);
        }
        else
            anim.SetBool("isFalling", false);
      
        // 점프 횟수 카운트
        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpCnt--;
        }
        // 점프 횟수 초기화
        if(isGround)
        {
            jumpCnt = jumpCount;
            anim.SetBool("isJumping", false);
        }

        // 방향 전환
        if (Input.GetButton("Horizontal"))
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

        float h = Input.GetAxis("Horizontal"); // 키보드 입력값
        
        // 이동 상태 여부
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // 항상 최대 속도로 설정
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // 무브 애니메이션
        anim.SetBool("isWalking", isMoving);

        if (curTime <= 0)
        {
            // 'Z' 키를 사용해 공격
            if (Input.GetKey(KeyCode.X))
            {
                // 땅에 있을 때 공격하면 멈춤
                if(isGround == true)
                {
                    speed = 0;
                }
                
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                // 공격 범위 안에서 공격하면 디버그 로그 출력
                foreach (Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().EnemyHit(1);
                    }
                    else if(collider.tag == "Boss1")
                    {
                        collider.GetComponent<Boss1_Health>().Boss1Hit(1);
                    }
                }

                curTime = coolTime;
                comboCount++;

                // 콤보 공격
                if (comboCount == 1)
                {
                    anim.SetTrigger("attack");
                    PlaySound("ATTACK");
                    Debug.Log("첫번째 콤보");
                }
                else if (comboCount == 2)
                {
                    foreach (Collider2D collider in collider2Ds)
                    {
                        if (collider.tag == "Enemy")
                        {
                            // 두번째 콤보는 데미지를 2로 설정
                            collider.GetComponent<Enemy>().EnemyHit(2);
                        }
                    }
                    anim.SetTrigger("attack2");
                    audioSource.clip = audioAttack2;
                    audioSource.Play();
                    Debug.Log("두번째 콤보");
                }
                else if (comboCount == 3)
                {
                    foreach (Collider2D collider in collider2Ds)
                    {
                        if (collider.tag == "Enemy")
                        {
                            // 세번째 콤보는 데미지를 3로 설정
                            collider.GetComponent<Enemy>().EnemyHit(3);
                        }
                    }
                    anim.SetTrigger("attack3");
                    PlaySound("ATTACK");
                    // 3타에 검기 소환
                    //Instantiate(SwordBeam, pos.position, transform.rotation);
                    Debug.Log("세번째 콤보");
                }
                else if (comboCount >= 4)
                {
                    ResetCombo();
                }
            }

            comboTime += Time.deltaTime;
            //Debug.Log("콤보 타임" + comboTime);

            // 콤보 시간이 넘어가면 콤보 카운트 리셋
            if (comboTime >= comboResetTime)
            {
                ResetCombo();
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        // 공격 멈추면 다시 이동 가능
        if(Input.GetKeyUp(KeyCode.X))
        {
            speed = 5;
        }

        dashFilltime += Time.deltaTime;

        //대쉬 쿨타임 돌면 시간초 고정
        if (dashFilltime >= dashCooltime)
        {
            dashFilltime = dashCooltime;    
        }

        // 대쉬
        if (dashFilltime < dashCooltime && Input.GetKeyDown(KeyCode.C) && isMoving == true)
        {
            isDash = false;
        }
        else
        {
            if (dashFilltime >= dashCooltime && Input.GetKeyDown(KeyCode.C) && isMoving == true)
            {
                // 구르기 시 무적
                gameObject.layer = 11;
                anim.SetTrigger("dash");
                audioSource.clip = audioDash;
                audioSource.Play();
                dashFilltime = 0;
                isDash = true;

                // 0.4초 동안
                Invoke("OffDamaged", 0.4f);
            }
        }

        // 대쉬 시간 및 속도 설정
        if (dashTime <= 0)
        {
            maxSpeed = speed;
            if (isDash)
            {
                dashTime = defaultTime;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            maxSpeed = dashSpeed;
        }
        isDash = false;
        
    }

    // 콤보 리셋
    void ResetCombo()
    {
        comboCount = 0;
        comboTime = 0;
    }

    // 공격 범위 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) // 피격
    {
        // 플레이어가 몬스터랑 접촉시
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }

        else if(collision.gameObject.tag == "Spike")
        {
            FullDamaged(collision.transform.position);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point 획득
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if(isBronze)
                gameManager.stagePoint += 50;
            else if(isSilver)
                gameManager.stagePoint += 100;
            else if(isGold)
                gameManager.stagePoint += 300;
            // Item 삭제
            collision.gameObject.SetActive(false);

            // Sound
            PlaySound("ITEM");
        }
        /*else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
            // Sound
            PlaySound("FINISH");
        }
        else if (collision.gameObject.tag == "Finish_back")
        {
            // Prev Stage
            gameManager.PrevStage();
            // Sound
            PlaySound("FINISH");
        }*/
        else if (collision.gameObject.tag == "ScenePotal")
        {
            PlaySound("FINISH");
            LoadNextScene();
        }

        else if (collision.gameObject.tag == "SavePoint")
        {
            GameObject SavaPointObject = GameObject.FindGameObjectWithTag("SavePoint");
            SavePoint = SavaPointObject.transform;
            Debug.Log("세이브 성공");
            collision.gameObject.SetActive(false);
        }
    }
    public void OnDamaged(Vector2 targetPos) // 피격시 설정
    {
        // 체력 감소
        gameManager.HealthDown();

        // 레이어를 PlayDameged로 변경
        gameObject.layer = 11;

        // 피격시 색상 및 투명도 설정
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 피격시 밀려남
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // 애니메이션
        anim.SetTrigger("doDamaged");
        PlaySound("DAMAGED"); // Sound
        Invoke("OffDamaged", 1); // Player 레이어로 돌아가는 시간(무적시간설정)
    }
    public void OffDamaged() // 레이어를 Player로 변경
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie() // 플레이어 사망시
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("DIE"); // Sound
        Invoke("TimeStop", 1);
    }

    public void OnLive() // 플레이어 리스폰으로 살아날 때
    {
        spriteRenderer.color = new Color(1, 1, 1, 1.0f);
        spriteRenderer.flipY = false;
        capsuleCollider.enabled = true;
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    public void LoadNextScene()
    {
        // 현재 씬의 빌드 인덱스를 가져옴
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // 빌드 순서에 있는 다음 씬으로 이동
        int nextSceneBuildIndex = (currentSceneBuildIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // 던전 씬으로 이동
        SceneManager.LoadScene(nextSceneBuildIndex);
    }

    public void FullDamaged(Vector2 targetPos) // 즉사
    {
        // 체력 감소
        gameManager.AllHealthDown();

        // 레이어를 PlayDameged로 변경
        gameObject.layer = 11;

        // 피격시 색상 및 투명도 설정
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 피격시 밀려남
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // 애니메이션
        anim.SetTrigger("doDamaged");
        PlaySound("DAMAGED"); // Sound
        Invoke("TimeStop", 1);
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

}
