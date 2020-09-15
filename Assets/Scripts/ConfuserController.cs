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
    
    //Used to toggle movement
    public bool IsPlayerNearby() { return Vector3.Distance(transform.position, playerShip.transform.position) <= detectionRange; }
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
        //invert movement if player is behind
        if (Vector3.Dot(transform.forward, playerShip.transform.position - transform.position) < 0f)
            moveAmountThisFrame *= -1;
        //apply the force
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        rb.AddForce(moveDirection);
    }

    void TurnShip()
    {
        //direct rotation of ship instead of force
        float turnAmountThisFrame = turnSpeed;
        //invert rotation depending on which side player is on
        if (Vector3.Dot(transform.right, playerShip.transform.position - transform.position) < 0f)
            turnAmountThisFrame *= -1;
        //do the rotation
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
