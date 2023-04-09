using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int _health;

    public void TakeDamage()
    {
        _health--;

        if (_health <= 0)
            Destroy(gameObject);
    }
}
