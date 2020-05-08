using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] Collider2D damageArea;
    [SerializeField] LayerMask  damageMask;
    [SerializeField] float      damage = 1;

    ContactFilter2D contactFilter;
    HP              hpComponent;

    public delegate void OnDealtDamage(HP target, float damage);
    public event OnDealtDamage onDamageDealt;

    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(damageMask);
        contactFilter.useTriggers = true;

        hpComponent = GetComponent<HP>();
        if (hpComponent == null)
        {
            hpComponent = GetComponentInParent<HP>();
        }
    }

    void Update()
    {
        if (hpComponent)
        {
            if (hpComponent.isInvulnerable) return;
        }

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
                    if (hp.DealDamage(damage))
                    {
                        if (onDamageDealt != null) onDamageDealt(hp, damage);
                    }
                }
            }
        }
    }
}
