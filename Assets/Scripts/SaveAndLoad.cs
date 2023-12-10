using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad Instance;
    public SaveData saveData;
    public GameManager gameManager;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitializeSaveData() // SaveData 초기화
    {
        saveData = new SaveData();
    }

    public void SaveReData() // 데이터 최신화
    {
        saveData.playerHealth = gameManager.health;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Vector3 playerPosition = playerObject.transform.position;
            saveData.playerPosition = new SerializableVector3(playerPosition.x, playerPosition.y, playerPosition.z);
        }
        saveData.sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

    public void SaveDataToJson() // Save 버튼 기능
    {
        SaveReData();
        string jsonData = JsonUtility.ToJson(saveData,true); // 데이터를 json 형식으로 변환
        string path = Path.Combine(Application.dataPath, "SaveData.json"); // 파일 경로 설정
        File.WriteAllText(path, jsonData); // 파일 생성
        Debug.Log("세이브 성공!"); // 세이브 성공 시 출력
    }

    /*public void SavaDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "SaveData.json"); // 가져올 파일 경로
        string jsonData = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(jsonData);

    }

    /*public void LoadData() // 저장한 데이터 로드
    {
        SavaDataFromJson();
        Debug.Log("Loaded Health: " + saveData.playerHealth);
        Debug.Log("Loaded Position: " + saveData.playerPosition.x + ", " + saveData.playerPosition.y + ", " + saveData.playerPosition.z);

        GameManager.Instance.health = saveData.playerHealth;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if(playerObject != null)
        {
            Transform playertransform = playerObject.transform;
            playertransform.position = saveData.playerPosition.ToVector3();
        }
        else
        {
            Debug.LogWarning("플레이어 오브젝트를 찾을 수 없습니다!");
        }

    }*/ //게임 매니저로 옮김
}

[System.Serializable]
public class SaveData // 세이브 데이터
{
    public int playerHealth; // 플레이어 체력
    public SerializableVector3 playerPosition; // 플레이어 위치
    public int sceneNumber; // 현재 씬
}

[System.Serializable]
public class SerializableVector3 // 위치 정보 받아오기
{
    public float x, y, z;
    
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}