using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour
{
    ParticleSystem particleLauncher = null;

    private void Awake()
    {
        particleLauncher = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0f)
        {
            particleLauncher.Emit(1);
        }
    }
}
