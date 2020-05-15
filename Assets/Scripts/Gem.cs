using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Pickup
{
    public int score;

    override protected void OnPickup(Player player)
    {
        player.AddScore(score);

        base.OnPickup(player);
    }

}
