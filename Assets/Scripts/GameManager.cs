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
    public GameObject[] Stages;
    public void NextStage()
    {
        // 스테이지 바꾸기
        if (stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else { // 게임 클리어시
            // 플레이어 컨트롤 잠금
            Time.timeScale = 0;
            //결과 UI
            Debug.Log("게임 클리어!");
        }
        // Calculate Point
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
            // 플레이어를 땅 위로 이동
            if (health > 1)
                PlayerReposition();

            // 떨어지면 체력감소
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-6, 4, -1);
        player.VelocityZero();
    }
}
