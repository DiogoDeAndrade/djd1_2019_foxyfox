using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public float    hp = 100;
    public float    invulnerableTime = 2.0f;

    float timer = 0.0f;

    public delegate void OnHit();
    public event OnHit onHit;

    public delegate void OnDead();
    public event OnDead onDead;

    public bool isInvulnerable
    {
        get
        {
            return timer > 0;
        }
        set
        {
            if (value) timer = invulnerableTime;
            else timer = 0;
        }
    }

    private void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public bool DealDamage(float damage)
    {
        if (isInvulnerable) return false;

        if (hp <= 0) return false;

        hp = hp - damage;

        if (hp <= 0)
        {
            if (onDead != null) onDead();
        }
        else
        {
            isInvulnerable = true;
            if (onHit != null) onHit();
        }

        return true;
    }
}
