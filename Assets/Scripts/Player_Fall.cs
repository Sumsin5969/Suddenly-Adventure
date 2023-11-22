using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player_Fall : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어를 땅 위로 이동
            if (gameManager.health > 1)
                gameManager.PlayerReposition();

            // 떨어지면 체력감소
            gameManager.HealthDown();
        }
    }
}
