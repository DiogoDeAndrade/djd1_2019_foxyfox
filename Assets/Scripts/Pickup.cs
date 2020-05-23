using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            SoundMng.instance.PlaySound(pickupSound, 0.5f);

            OnPickup(player);
        }
    }

    virtual protected void OnPickup(Player player)
    {
        Destroy(gameObject);
    }
}
