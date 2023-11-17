using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 공격 기능
public class Boss_Attack : MonoBehaviour
{
    public int attackDamage = 1;
    public int enrageAttackDamage = 2;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        // 공격 범위 내에서 내려찍는 타이밍에 피격 함수 실행
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerMove>().OnDamaged(colInfo.transform.position);
        }
    }
}
