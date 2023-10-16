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
        // �������� �ٲٱ�
        if (stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else { // ���� Ŭ�����
            // �÷��̾� ��Ʈ�� ���
            Time.timeScale = 0;
            //��� UI
            Debug.Log("���� Ŭ����!");
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
            // �÷��̾� ����
            player.OnDie();

            //  ��� UI
            Debug.Log("�׾����ϴ�!");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �÷��̾ �� ���� �̵�
            if (health > 1)
                PlayerReposition();

            // �������� ü�°���
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-6, 4, -1);
        player.VelocityZero();
    }
}
