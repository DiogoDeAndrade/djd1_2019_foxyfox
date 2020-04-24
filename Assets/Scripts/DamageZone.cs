using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] Collider2D damageArea;
    [SerializeField] LayerMask  damageMask;
    [SerializeField] float      damage = 100;

    ContactFilter2D contactFilter;

    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(damageMask);
        contactFilter.useTriggers = true;
    }

    void Update()
    {
        Collider2D[] results = new Collider2D[64];

        int nCollisions = Physics2D.OverlapCollider(damageArea, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                Collider2D otherCollider = results[i];

                HP hp = otherCollider.GetComponent<HP>();

                if (hp)
                {
                    hp.DealDamage(damage);
                }
            }
        }
    }
}
