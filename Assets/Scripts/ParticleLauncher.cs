﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour
{
    ParticleSystem particleLauncher = null;

    [SerializeField] int particlePercentChance = 50;

    private void Awake()
    {
        particleLauncher = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0f)
        {
            if (Random.Range(0, 100) <= particlePercentChance)
                particleLauncher.Emit(1);
        }
    }
}
