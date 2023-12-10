using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 파괴
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 현재 씬에서 AllMenu 오브젝트를 찾아보고, 있다면 파괴
        GameObject existingAllMenu = GameObject.Find("All Menu");

        if (existingAllMenu != null && existingAllMenu != gameObject)
        {
            // 다른 AllMenu 오브젝트가 이미 존재하면 파괴
            Destroy(existingAllMenu);
        }
    }
}
