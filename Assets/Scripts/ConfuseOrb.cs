using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseOrb : MonoBehaviour
{
    Rigidbody rb = null;
    [SerializeField] ParticleSystem ps = null;
    [SerializeField] GameObject art = null;

    [Header("Projectile Variables")]
    [SerializeField] float velocity = 1f;
    [SerializeField] float lifespan = 3f;
    [SerializeField] float effectDuration = 5f;

    bool hasCollided; //prevents duplicate collisions

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocity;
        DelayHelper.DelayAction(this, Despawn, lifespan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerShip playerShip = collision.gameObject.GetComponent<PlayerShip>();

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
        rb.Sleep();
        Destroy(GetComponent<Collider>());
        art.SetActive(false);
        Destroy(gameObject, ps.main.startLifetime.constant);
    }
}
