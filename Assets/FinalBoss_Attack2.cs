using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss_Attack2 : MonoBehaviour
{
    public int attackDamage = 1;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    public Vector3 attackOffset2;
    public float attackRange2 = 1f;

    private void Awake()
    {

    }

    public void Attack2()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Vector3 pos2 = transform.position;
        pos2 += transform.right * attackOffset2.x;
        pos2 += transform.up * attackOffset2.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        Collider2D colInfo2 = Physics2D.OverlapCircle(pos2, attackRange2, attackMask);


        // 공격 범위 내에서 내려찍는 타이밍에 피격 함수 실행
        if (gameObject.layer != 14 && colInfo != null)
        {
            colInfo.GetComponent<PlayerMove>().OnDamaged(colInfo.transform.position);
        }
        else if (gameObject.layer != 14 && colInfo2 != null)
        {
            colInfo2.GetComponent<PlayerMove>().OnDamaged(colInfo2.transform.position);
        }
    }

    void OnDrawGizmos()
    {
        // Attack 함수에서 계산한 오버랩 써클의 범위를 Scene 뷰에 그립니다.
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.color = Color.green; // 원하는 색상으로 설정
        Gizmos.DrawWireSphere(pos, attackRange);

        // Attack 함수에서 계산한 오버랩 써클의 범위를 Scene 뷰에 그립니다.
        Vector3 pos2 = transform.position;
        pos2 += transform.right * attackOffset2.x;
        pos2 += transform.up * attackOffset2.y;

        Gizmos.color = Color.green; // 원하는 색상으로 설정
        Gizmos.DrawWireSphere(pos2, attackRange2);
    }
}
