using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private const float FireDelay = .55f;

    private void Start()
    {
        Destroy(gameObject, FireDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage();
        }
    }
}
