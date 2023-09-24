using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal"); // 키보드 입력값

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        
        if (rigid.velocity.x > maxSpeed) // 오른쪽 최고 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최고 속도
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
