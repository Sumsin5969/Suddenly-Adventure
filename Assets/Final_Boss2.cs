using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Boss2 : MonoBehaviour
{
    public Transform player;
    public float moveRange;
    public float banishRange;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 색상 설정
        Gizmos.DrawWireSphere(transform.position, moveRange);

        Gizmos.color = Color.blue; // 색상 설정
        Gizmos.DrawWireSphere(transform.position, banishRange);
    }
}
