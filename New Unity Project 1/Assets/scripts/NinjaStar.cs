using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : IWeapon
{
    [SerializeField] protected Rigidbody2D[] Bullets;
    [SerializeField] protected Rigidbody2D Bullet;
    public float speed;               // The speed the bullet will fire at.
    public Transform bulletSpawnLoc;  // The position the bullet will fire from
    public float reloadTime;          // The time it takes to reload the weapon

    private PlayerController playerCtrl;       // Reference to the PlayerControl script.
    private Animator anim;                     // Reference to the Animator component.
    private AudioSource audio;                 // Reference to the Audio component.

    bool canFire = true;                       // Condition for that controls whether the player can shoot
    public bool CanShootNextBurst;             // Condition for if they can fire a burst of projectiles
    public float burstDelay;                   // Time between bursts

    void Awake()
    {
        
    }


    void Update()
    {
        if (!playerCtrl.isReloading)
        {
        // If the specified button for reload is pressed - disable firing and start coroutine
            if (Input.GetButtonDown("Reload_P" + (playerCtrl.Identifier + 1)))
            {
                playerCtrl.isReloading = true;
                StartCoroutine(playerCtrl.Reload(reloadTime)); // Coroutine called with a specified time to wait
            }

            if (CanShootNextBurst && canFire) //check if a player can fire a burst 
            {
                if (Input.GetAxis("Fire1_P" + (playerCtrl.Identifier + 1)) < -0.98f)
                {
                    playerCtrl.Ammo--;
                    anim.SetBool("Shooting", true);
                    audio.Play();
                    CanShootNextBurst = false;
                    StartCoroutine(Burst());
                    canFire = false;
                }
            }
        }
    }

    IEnumerator Burst()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerCtrl.IsFacingRight) { }
                // ... instantiate the rocket facing right and set it's velocity to the right. 
               // SpawnRightFacingBullet();
            else
                // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                //SpawnLeftFacingBullet();
            yield return new WaitForSeconds(.05f);
        }
        anim.SetBool("Shooting", false);
        StartCoroutine(BurstDelay());
    }

    IEnumerator BurstDelay()
    {
        yield return new WaitForSeconds(burstDelay);
        CanShootNextBurst = true;
    }
}
