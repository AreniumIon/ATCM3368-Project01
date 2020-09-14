using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : Powerup
{
    //SPEED VARIABLES
    [SerializeField] float speedIncreaseAmount = 20f;

    //on collision
    protected override void ActivatePowerup(PlayerShip playerShip)
    {
        if (playerShip != null)
        {
            // keep track of duplicate boosts
            playerShip.speedPowerupActive = true;
            // powerup player
            playerShip.SetSpeed(speedIncreaseAmount);
            // visuals
            playerShip.SetBoosters(true);
            //particles and audio
            playerShip.PlaySpeedStartEffect();
        }
    }

    //called when speed boost timer runs out
    protected override void DeactivatePowerup(PlayerShip playerShip)
    {
        RemoveDuplicateEffects(playerShip);
        // visuals
        playerShip?.SetBoosters(false);
        // keep track of duplicate boosts
        playerShip.speedPowerupActive = false;
        //audio
        playerShip.PlaySpeedEndEffect();
    }

    //searches for duplicate speed boost to remove its effects
    public override void FixDuplicatePowerups(PlayerShip playerShip)
    {
        foreach (PowerupSpeed s in transform.parent.GetComponentsInChildren<PowerupSpeed>())
            if (s.poweredUp) //if the speed boost is powered up, remove duplicate effects
                s.RemoveDuplicateEffects(playerShip);
    }

    //called when a second speed boost is activated, to deactivate the first ones effects without giving wrong audio/visual feedback
    public override void RemoveDuplicateEffects(PlayerShip playerShip)
    {
        //dont apply twice
        if (poweredUp)
        {
            // revert player powerup. - will subtract
            playerShip?.SetSpeed(-speedIncreaseAmount);
            // prevent deactivation from happening twice
            poweredUp = false;
        }
    }
}
