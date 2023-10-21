using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;

    void Update() // ���� ������Ʈ
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
    public void NextStage()
    {
        // �������� �ٲٱ�
        if (stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE" + (stageIndex + 1);
        }
        else { // ���� Ŭ�����
            // �÷��̾� ��Ʈ�� ���
            Time.timeScale = 0;
            //��� UI
            Debug.Log("���� Ŭ����!");
            //Retry ��ư UI
            UIRestartBtn.SetActive(true);
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            UIRestartBtn.SetActive(true);
        }
        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            // ��� Health UI OFF
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            // �÷��̾� ����
            player.OnDie();

            //  ��� UI
            Debug.Log("�׾����ϴ�!");
            // Retry ��ư UI
            UIRestartBtn.SetActive(true);
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

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
