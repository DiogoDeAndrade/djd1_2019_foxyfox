using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            OnPickup(player);            
        }
    }

    virtual protected void OnPickup(Player player)
    {
        Destroy(gameObject);
    }
}
