using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuserParticleLauncher : MonoBehaviour
{
    ParticleSystem particleLauncher = null;

    [SerializeField] int particlePercentChance = 50;

    private void Awake()
    {
        particleLauncher = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (GetComponentInParent<ConfuserController>().IsPlayerNearby())
        {
            if (Random.Range(0, 100) <= particlePercentChance)
                particleLauncher.Emit(1);
        }
    }
}
