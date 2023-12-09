using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject target; // 카메라가 따라갈 대상
    public float moveSpeed; // 카메라가 따라갈 속도
    private Vector3 targetPosition; // 대상의 현재 위치
    public float minX, maxX, minY, maxY; // 제한 범위

    void Update()
    {
        // 대상이 있는지 체크
        if (target.gameObject != null)
        {
            // this는 카메라를 의미 (z값은 카메라값을 그대로 유지)
            targetPosition.Set(target.transform.position.x, target.transform.position.y + 4f, this.transform.position.z);

            // vectorA -> B까지 T의 속도로 이동
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (target != null)
        {
            // 대상의 위치에 따라 카메라의 목표 위치 설정
            targetPosition.Set(Mathf.Clamp(target.transform.position.x, minX, maxX),
                                Mathf.Clamp(target.transform.position.y + 4f, minY, maxY),
                                transform.position.z);

            // 목표 위치로 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
