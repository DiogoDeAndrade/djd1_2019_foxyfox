using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public float hp = 100;

    public delegate void OnDead();
    public event OnDead onDead;

    public void DealDamage(float damage)
    {
        if (hp <= 0) return;

        hp = hp - damage;

        if (hp <= 0)
        {
            onDead();
        }
    }
}
