using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    //POWERUP VARIABLES
    [Header("Powerup Settings")]
    [SerializeField] protected float powerupDuration = 5f;

    [Header("Setup")]
    [SerializeField] protected GameObject visualsToDeactivate = null;

    protected Collider colliderToDeactivate = null;
    protected bool poweredUp = false;

    private void Awake()
    {
        colliderToDeactivate = GetComponent<Collider>();

        EnableObject();
    }

    //ACTIVATION
    private void OnTriggerEnter(Collider other)
    {
        PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();

        if (playerShip != null && !poweredUp)
        {
            //start powerup timer, restart if already started
            PowerupSequence(playerShip);
        }
    }
    
    void PowerupSequence(PlayerShip playerShip)
    {
        //duplicate handling
        FixDuplicatePowerups(playerShip);

        //set boolean for detecting lockout
        poweredUp = true;

        ActivatePowerup(playerShip);
        //simulate this object being disabled
        DisableObject();

        //delay end of powerup
        DelayHelper.DelayAction(this, EndPowerupSequence, playerShip, powerupDuration);
    }

    //DEACTIVATION
    void EndPowerupSequence(PlayerShip playerShip)
    {
        //remove effects (only if not previously removed by duplicate)
        if (poweredUp)
        {
            DeactivatePowerup(playerShip);
            poweredUp = false;
        }

        //reset object
        EnableObject();
    }

    //Powerup disappears
    public void DisableObject()
    {
        //disable collider
        colliderToDeactivate.enabled = false;
        //disable visuals
        visualsToDeactivate.SetActive(false);
        //TODO deactivate particle flash/audio

    }

    //Powerup reappears
    public void EnableObject()
    {
        //enable collider
        colliderToDeactivate.enabled = true;
        //enable visuals
        visualsToDeactivate.SetActive(true);
        //TODO reactivate particle flash/audio
    }


    //ABSTRACT METHODS
    //on collision
    protected abstract void ActivatePowerup(PlayerShip playerShip);

    //called when speed boost timer runs out
    protected abstract void DeactivatePowerup(PlayerShip playerShip);

    //searches for duplicate speed boost to remove its effects
    public abstract void FixDuplicatePowerups(PlayerShip playerShip);

    //called when a second speed boost is activated, to deactivate the first ones effects without giving wrong audio/visual feedback
    public abstract void RemoveDuplicateEffects(PlayerShip playerShip);

}
