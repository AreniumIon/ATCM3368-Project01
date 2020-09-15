using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinVolume : MonoBehaviour
{
    [SerializeField] GameController gameController = null;

    private void OnTriggerEnter(Collider other)
    {
        //if collides with player ship, kill it
        PlayerShip playerShip = other.GetComponent<PlayerShip>();
        if (playerShip != null)
        {
            gameController.Win();
        }
    }
}
