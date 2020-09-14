using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //if collides with player ship, kill it
        PlayerShip playerShip = other.GetComponent<PlayerShip>();
        if (playerShip != null && !playerShip.invinciblePowerupActive)
        {
            playerShip.Kill();
            DelayHelper.DelayAction(this, playerShip.Respawn, 2f);
        }
    }
}
