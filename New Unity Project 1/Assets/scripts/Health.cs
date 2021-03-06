﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Event script to handle player death by broadcasting event to gamemanager when player dies
    public delegate void CheckForKillPlayer();
    public static event CheckForKillPlayer OnCheckKillPlayer;

    public const float maxHealth = 1;
    public float currentHealth = maxHealth;
    public bool poisoned;
    float poisonTimer = 1f;

    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnCheckKillPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (poisoned)
            currentHealth -= Time.deltaTime / 4;
    }

    private void Update()
    {
        if (poisoned)
        {
            if (poisonTimer > 0)
            {
                poisonTimer -= Time.deltaTime;
                OnCheckKillPlayer();
            }
            else
            {
                poisoned = !poisoned;
                ResetPoison();
            }
        }
    }

    public void ResetPoison()
    {
        poisonTimer = 1f;
    }
}
