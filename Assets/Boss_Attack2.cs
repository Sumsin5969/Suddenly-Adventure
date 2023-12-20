using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

// 보스 공격 기능
public class Boss_Attack2 : MonoBehaviour
{
    public int attackDamage2 = 1;
    public int enrageAttackDamage2 = 2;

    public Vector3 attackOffset2;
    public float attackRange2 = 0.5f;
    public LayerMask attackMask2;

    public void Attack2()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset2.x;
        pos += transform.up * attackOffset2.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange2, attackMask2);

        // 공격 범위 내에서 휘두르는 타이밍에 피격 함수 실행
        if (gameObject.layer != 14 && colInfo != null)
        {
            colInfo.GetComponent<PlayerMove>().OnDamaged(colInfo.transform.position, 1);
        }
    }

    void OnDrawGizmos()
    {
        // Attack 함수에서 계산한 오버랩 써클의 범위를 Scene 뷰에 그립니다.
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset2.x;
        pos += transform.up * attackOffset2.y;

        Gizmos.color = Color.red; // 원하는 색상으로 설정
        Gizmos.DrawWireSphere(pos, attackRange2);
    }
}
