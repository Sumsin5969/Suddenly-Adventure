using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public void NextStage()
    {
        stageIndex++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
            health --;
        else {
            // 플레이어 죽음
            player.OnDie();

            //  결과 UI
            Debug.Log("죽었습니다!");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 떨어지면 체력감소
            HealthDown();

            // 플레이어를 땅 위로 이동
            if (health > 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector3(-6, 4, -1); // 어디로 이동시킬지
            }
        }
    }
}
