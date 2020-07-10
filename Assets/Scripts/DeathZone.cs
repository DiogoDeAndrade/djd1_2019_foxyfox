using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (player)
        {
            HP hp = player.GetComponent<HP>();
            if (hp)
            {
                hp.DealDamage(1);
                if (hp.hp > 0)
                {
                    player.GotoSafe();
                }
            }
        }
    }
}
