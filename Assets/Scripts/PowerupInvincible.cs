using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvincible : Powerup
{
    //INVINCIBILITY VARIABLES

    //on collision
    protected override void ActivatePowerup(PlayerShip playerShip)
    {
        if (playerShip != null)
        {
            // powerup player and keep track of duplicate powerups
            playerShip.invinciblePowerupActive = true;
            //particles and audio
            playerShip.PlayInvincibleStartEffect();
            // visuals
            playerShip.SetInvincibleMaterial(true);
        }
    }

    //called when invincible timer runs out
    protected override void DeactivatePowerup(PlayerShip playerShip)
    {
        RemoveDuplicateEffects(playerShip);
        // powerdown and keep track of duplicate powerups
        playerShip.invinciblePowerupActive = false;
        //audio
        playerShip.PlayInvincibleEndEffect();
        // visuals
        playerShip?.SetInvincibleMaterial(false);
    }

    //searches for duplicate invincible powerups to remove its effects
    public override void FixDuplicatePowerups(PlayerShip playerShip)
    {
        foreach (PowerupInvincible i in transform.parent.GetComponentsInChildren<PowerupInvincible>())
            if (i.poweredUp) //if the speed boost is powered up, remove duplicate effects
                i.RemoveDuplicateEffects(playerShip);
    }

    //called when a second speed boost is activated, to deactivate the first ones effects without giving wrong audio/visual feedback
    public override void RemoveDuplicateEffects(PlayerShip playerShip)
    {
        //dont apply twice
        if (poweredUp)
        {
            // prevent deactivation from happening twice
            poweredUp = false;
        }
    }
}
