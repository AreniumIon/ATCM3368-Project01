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
    [SerializeField] GameObject art = null;
    [SerializeField] LayerMask layerMask;
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
    
    //Used to toggle movement
    bool IsPlayerNearby() { return Vector3.Distance(transform.position, playerShip.transform.position) <= detectionRange; }
    //Used to shoot
    bool IsPlayerInRange()
    {
        RaycastHit hit;
        return Physics.Raycast(projectileSpawnPoint.position, transform.forward, out hit, distanceToFire, layerMask);
    }



    //MOVEMENT
    void MoveShip()
    {
        //move towards player
        float moveAmountThisFrame = moveSpeed;
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        rb.AddForce(moveDirection);
    }

    void TurnShip()
    {
        //direct rotation of ship instead of force
        float turnAmountThisFrame = turnSpeed;
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    //DYING
    public void Kill()
    {
        this.gameObject.SetActive(false);
    }
    
    //FIRE PROJECTILE
    void Shoot()
    {
        Instantiate(confuseProjectile, projectileSpawnPoint.position, transform.rotation, projectileParent);
        PlayShootFeedback();
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
