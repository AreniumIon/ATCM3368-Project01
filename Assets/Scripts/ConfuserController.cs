using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuserController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 3f;
    [SerializeField] float detectionRange = 10f;

    [Header("Projectile")]
    [SerializeField] float projectileReloadTime = 3f;
    [SerializeField] float distanceToFire = 5f;
    [SerializeField] GameObject confuseProjectile = null;
    [SerializeField] AudioClip projectileFireSound = null;
    [SerializeField] Transform projectileSpawnPoint = null;

    [Header("Other")]
    Rigidbody rb = null;
    Transform projectileParent = null;
    PlayerShip playerShip = null;
    [SerializeField] LayerMask layerMask = new LayerMask();
    bool canFire = true; //cooldown for firing

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        projectileParent = GameObject.FindGameObjectWithTag("ProjectilesParent").transform;
        playerShip = Transform.FindObjectOfType<PlayerShip>();
    }

    //RESPOND TO PLAYER
    private void FixedUpdate()
    {
        //moves if close to player
        if (IsPlayerNearby())
        {
            MoveShip();
            TurnShip();
            //shooting projectile
            if (canFire && IsPlayerInRange())
                Shoot();
        }
    }
    
    //Decides if ship should move
    public bool IsPlayerNearby()
    {
        return Vector3.Distance(transform.position, playerShip.transform.position) <= detectionRange;
    }
    //Decides if ship should shoot
    bool IsPlayerInRange()
    {
        RaycastHit hit;
        return Physics.Raycast(projectileSpawnPoint.position, transform.forward, out hit, distanceToFire, layerMask);
    }



    //MOVEMENT
    void MoveShip()
    {
        //calculate force
        float moveAmountThisFrame = GetDot(moveSpeed, transform.forward);
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        //apply force
        rb.AddForce(moveDirection);
    }

    void TurnShip()
    {
        //calculate rotation
        float turnAmountThisFrame = GetDot(turnSpeed, transform.right);
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        //apply rotation
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    //used for MoveShip() and TurnShip()
    float GetDot(float variableSpeed, Vector3 baseDirection)
    {
        //invert result depending on which side its on
        if (Vector3.Dot(baseDirection, playerShip.transform.position - transform.position) < 0f)
            variableSpeed *= -1;
        return variableSpeed;
    }

    //DYING
    public void Kill()
    {
        this.gameObject.SetActive(false);
    }
    
    //FIRE PROJECTILE
    void Shoot()
    {
        //create projectile
        Instantiate(confuseProjectile, projectileSpawnPoint.position, transform.rotation, projectileParent);
        //play projectile effects
        PlayShootFeedback();
        //prevents from shooting again until DelayAction marks the ship as reloaded
        canFire = false;
        DelayHelper.DelayAction(this, ReadyToFire, projectileReloadTime);
    }

    void PlayShootFeedback()
    {
        //sound
        AudioHelper.PlayClip2D(projectileFireSound, .2f);
    }

    void ReadyToFire()
    {
        canFire = true;
    }
}
