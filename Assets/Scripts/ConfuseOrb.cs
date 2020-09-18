using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseOrb : MonoBehaviour
{
    [SerializeField] ParticleSystem particles = null;
    [SerializeField] GameObject art = null;

    [Header("Projectile Variables")]
    [SerializeField] float velocity = 1f;
    Vector3 velocityVector;
    [SerializeField] float lifespan = 3f;
    [SerializeField] float effectDuration = 5f;

    bool hasCollided; //prevents duplicate collisions

    public void Start()
    {
        velocityVector = Vector3.forward * velocity;
        DelayHelper.DelayAction(this, Despawn, lifespan);
    }

    public void Update()
    {
        transform.Translate(velocityVector * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();

        if (!hasCollided && playerShip != null)
        {
            hasCollided = true;
            //effects
            playerShip.ActivateConfused(effectDuration);
            //Despawn
            Despawn();
        }
    }

    void Despawn()
    {
        //visuals
        art.SetActive(false);
        //disable more particles spawning
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        //disable collider
        GetComponent<Collider>().enabled = false;
        //will destroy self when particles are gone
        Destroy(gameObject, particles.main.startLifetime.constant);
    }
}
