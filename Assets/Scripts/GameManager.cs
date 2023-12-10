using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public LoadData loadData;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Update() // 점수 업데이트
    {
        // 수정자: 정성헌 수정일: 11/21
        // 무한불러오기로 콘솔창이 시끄러워서 조건문추가함
        if (UIPoint != null)
            UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    /*public void PrevStage()
    {
        if (stageIndex < Stages.Length)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex--;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
    }*/
    public void NextStage()
    {
        // 스테이지 바꾸기
        if (stageIndex < SceneManager.sceneCount)
        {
            stageIndex++;
            string sceneName = SceneManager.GetSceneAt(stageIndex).name;
            UIStage.text = sceneName;
        }
        /*else
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
        stagePoint = 0;*/
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

    public void AllHealthDown()
    {
        health = 0;
        // 모든 Health UI OFF
        UIhealth[0].color = new Color(1, 0, 0, 0.4f);
        UIhealth[1].color = new Color(1, 0, 0, 0.4f);
        UIhealth[2].color = new Color(1, 0, 0, 0.4f);
        // 플레이어 죽음
        player.OnDie();

        //  결과 UI
        Debug.Log("죽었습니다!");
        // Retry 버튼 UI
        UIRestartBtn.SetActive(true);

    }

    public void PlayerReposition()
    {
        player.transform.position = player.SavePoint.position;
        player.VelocityZero();
    }

    public void RestartDungeon()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("던전");
    }

    public void ReSpawn() // Retry 버튼 기능
    {
        health = 3;
        Time.timeScale = 1;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Transform playertransform = playerObject.transform;
        playertransform.position = player.SavePoint.position;
        player.OnLive();
        player.OffDamaged();
        UIRestartBtn.SetActive(false); // Retry 버튼 비활성화
        // 모든 Health UI ON
        UIhealth[0].color = new Color(1, 1, 1, 1.0f);
        UIhealth[1].color = new Color(1, 1, 1, 1.0f);
        UIhealth[2].color = new Color(1, 1, 1, 1.0f);
    }

    public void SavaDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "SaveData.json"); // 가져올 파일 경로
        string jsonData = File.ReadAllText(path);
        loadData = JsonUtility.FromJson<LoadData>(jsonData);

    }
    public void LoadSaveData()
    {
        SavaDataFromJson();
        LoadAllData();
    }

    public void LoadAllData()
    {
        int nowsceneindex = SceneManager.GetActiveScene().buildIndex;
        if (nowsceneindex == loadData.sceneNumber)
        {
            Debug.Log("Loaded Health: " + loadData.playerHealth);
            Debug.Log("Loaded Position: " + loadData.playerPosition.x + ", " + loadData.playerPosition.y + ", " + loadData.playerPosition.z);

            health = loadData.playerHealth;
            if (health > 2)
            {
                UIhealth[0].color = new Color(1, 1, 1, 1.0f);
                UIhealth[1].color = new Color(1, 1, 1, 1.0f);
                UIhealth[2].color = new Color(1, 1, 1, 1.0f);
            }
            else if (health > 1)
            {
                UIhealth[0].color = new Color(1, 1, 1, 1.0f);
                UIhealth[1].color = new Color(1, 1, 1, 1.0f);
                UIhealth[2].color = new Color(1, 0, 0, 0.4f);
            }
            else if (health > 0)
            {
                UIhealth[0].color = new Color(1, 1, 1, 1.0f);
                UIhealth[1].color = new Color(1, 0, 0, 0.4f);
                UIhealth[2].color = new Color(1, 0, 0, 0.4f);
            }
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Transform playertransform = playerObject.transform;
            playertransform.position = loadData.playerPosition.ToVector3();
        }
        else
        {
            Debug.LogError("해당 맵에서는 할 수 없습니다!");
        }
    }

}

[System.Serializable]
public class LoadData // 로드 데이터
{
    public int playerHealth; // 플레이어 체력
    public SerializableVector3 playerPosition; // 플레이어 위치
    public int sceneNumber; // 현재 씬
}
