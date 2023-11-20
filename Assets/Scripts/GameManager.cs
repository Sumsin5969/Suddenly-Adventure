using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 현재 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
    void Update() // 점수 업데이트
    {
        // 수정자: 정성헌 수정일: 11/21
        // 무한불러오기로 콘솔창이 시끄러워서 조건문추가함
        if (UIPoint != null)
            UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void PrevStage()
    {
        if (stageIndex < Stages.Length)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex--;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
    }
    public void NextStage()
    {
        // 스테이지 바꾸기
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        { // 게임 클리어시
            // 플레이어 컨트롤 잠금
            Time.timeScale = 0;
            //결과 UI
            Debug.Log("게임 클리어!");
            //Retry 버튼 UI
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
            // 모든 Health UI OFF
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            // 플레이어 죽음
            player.OnDie();

            //  결과 UI
            Debug.Log("죽었습니다!");
            // Retry 버튼 UI
            UIRestartBtn.SetActive(true);
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

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void RestartDungeon()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Dungeon");
    }
}
