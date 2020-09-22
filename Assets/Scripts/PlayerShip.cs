using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    //Movement variables
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float turnSpeed = 3f;

    [Header("Powerup Speed Feedback")]
    [SerializeField] TrailRenderer speedTrail = null;
    [SerializeField] ParticleSystem speedParticle = null;
    [SerializeField] AudioClip speedStartSound = null;
    [SerializeField] AudioClip speedEndSound = null;
    public bool speedPowerupActive;

    [Header("Powerup Invincible Feedback")]
    [SerializeField] Material invincibleMaterial = null;
    [SerializeField] Material hullMaterial = null;
    [SerializeField] Material highlightsMaterial = null;
    [SerializeField] Material cockpitMaterial = null;
    [SerializeField] ParticleSystem invincibleParticle = null;
    [SerializeField] AudioClip invincibleStartSound = null;
    [SerializeField] AudioClip invincibleEndSound = null;
    public bool invinciblePowerupActive;

    [Header("Confused Feedback")]
    [SerializeField] ParticleSystem confusedParticle = null;
    [SerializeField] AudioClip confusedStartSound = null;
    [SerializeField] AudioClip confusedEndSound = null;
    [SerializeField] Material cockpitConfusedMaterial = null;

    [Header("Confused Variables")]
    [SerializeField] float confuseForceMax = .5f;
    [SerializeField] float confuseForceMin = .3f;
    [SerializeField] float confuseRotationMax = .5f;
    [SerializeField] float confuseRotationMin = .3f;
    float confuseForce = 0f;
    float confuseRotation = 0f;
    bool isConfused = false;
    [SerializeField] float confusedChangeTime = 1f; //how often the confused amount changes

    [Header("Other Variables")]
    Rigidbody rb = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] GameObject deathParticle = null;
    [SerializeField] GameObject loseText = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        speedTrail.enabled = false;

        //start confused calculation chain
        SetConfuseForce();
        SetConfuseRotation();
    }

    //MOVEMENT
    private void FixedUpdate()
    {
        MoveShip();
        TurnShip();
    }

    //force
    void MoveShip()
    {
        float vInput = Input.GetAxisRaw("Vertical");
        //apply confusion force
        if (isConfused)
            vInput += confuseForce;
        //apply input as force to ship
        float moveAmountThisFrame = vInput * moveSpeed;
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        rb.AddForce(moveDirection);
    }

    //rotation
    void TurnShip()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        //apply confusion force
        if (isConfused)
            hInput += confuseRotation;
        //direct rotation of ship instead of force
        float turnAmountThisFrame = hInput * turnSpeed;
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    //DYING/RESPAWNING
    public void Kill()
    {
        //audio
        AudioHelper.PlayClip2D(deathSound, .2f);
        //text
        loseText.SetActive(true);
        //visual
        GameObject newDeathParticle = Instantiate(deathParticle, transform.position, Quaternion.identity);
        newDeathParticle.GetComponent<ParticleSystem>().Play();
        this.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        SceneManager.LoadScene(0);
    }

    //SPEED POWERUP
    public void SetSpeed(float speedChange) { moveSpeed += speedChange; }
    public void SetBoosters(bool activeState) { speedTrail.enabled = activeState; }

    public void PlaySpeedStartEffect()
    {
        //graphics
        speedParticle.Play();
        //sound
        AudioHelper.PlayClip2D(speedStartSound, .2f);
    }

    public void PlaySpeedEndEffect()
    {
        if (gameObject.activeSelf)
        {
            //graphics
            //sound
            AudioHelper.PlayClip2D(speedEndSound, .2f);
        }
    }

    //INVINCIBILITY POWERUP
    public void SetInvincibleMaterial(bool activeState)
    {
        //loops through meshes to decide material
        int meshNumber = 0;
        //start loop
        foreach (MeshRenderer m in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
        {
            if (activeState)
                m.material = invincibleMaterial;
            else
                switch (meshNumber++)
                {
                    case 0: case 1: //Hull meshes
                        m.material = hullMaterial;
                        break;
                    case 2: case 3: case 4: case 5: case 6: //Highlight meshes
                        m.material = highlightsMaterial;
                        break;
                    case 7: //Cockpit meshes
                        m.material = cockpitMaterial;
                        break;
                }
        }
    }

    public void PlayInvincibleStartEffect()
    {
        //graphics
        invincibleParticle.Play();
        //sound
        AudioHelper.PlayClip2D(invincibleStartSound, .2f);
        //remove confusion
        DeactivateConfused();
    }

    public void PlayInvincibleEndEffect()
    {
        if (gameObject.activeSelf)
        {
            //graphics
            invincibleParticle.Stop();
            //sound
            AudioHelper.PlayClip2D(invincibleEndSound, .2f);
        }
    }

    //CONFUSED EFFECT
    public void ActivateConfused(float duration)
    {
        //only activate confused effects if not already confused (prevents getting overwhelmed)
        if (!isConfused && !invinciblePowerupActive)
        {
            isConfused = true;
            //delay deactivation
            DelayHelper.DelayAction(this, DeactivateConfused, duration);
            //particles
            confusedParticle.Play();
            //sound
            AudioHelper.PlayClip2D(confusedStartSound, .2f);
            //cockpit turns red
            //'7': Cockpit ID
            transform.GetChild(0).GetComponentsInChildren<MeshRenderer>()[7].material = cockpitConfusedMaterial;
        }
    }

    void DeactivateConfused()
    {
        if (isConfused)
        {
            isConfused = false;
            //particles
            confusedParticle.Stop();
            //sound
            AudioHelper.PlayClip2D(confusedEndSound, .2f);
            //cockpit back to normal
            //'7': Cockpit ID
            transform.GetChild(0).GetComponentsInChildren<MeshRenderer>()[7].material = cockpitMaterial;
        }
    }

    //calculate confused effect every interval
    void SetConfuseForce()
    {
        //confine the force
        confuseForce = ConfineConfusion(GetNewConfuseAmount(confuseForceMax, confuseForceMin));
        DelayHelper.DelayAction(this, SetConfuseForce, confusedChangeTime);
    }

    void SetConfuseRotation()
    {
        //dont confine rotation
        confuseRotation = GetNewConfuseAmount(confuseRotationMax, confuseRotationMin);
        DelayHelper.DelayAction(this, SetConfuseRotation, confusedChangeTime);
    }

    //used by both methods above
    float GetNewConfuseAmount(float max, float min)
    {
        float rn = Random.Range(min, max);
        if (Random.Range(0, 2) == 0)
            rn *= -1;
        return rn;
    }
    
    //confines confusion variables to [-1, 1]
    float ConfineConfusion(float baseFloat)
    {
        if (baseFloat > 1)
            baseFloat = 1;
        else if (baseFloat < -1)
            baseFloat = -1;
        return baseFloat;
    }
}
