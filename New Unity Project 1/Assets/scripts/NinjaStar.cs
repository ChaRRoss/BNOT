using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : MonoBehaviour, IWeapon
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
    public float burstDelay;                   // Time between bursts


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

            if (canFire) //check if a player can fire
            {
                // if input trigger is pulled all the way down
                if (Input.GetAxis("Fire1_P" + (playerCtrl.Identifier + 1)) < -0.98f)
                {
                    playerCtrl.Ammo--;//decrease ammo count
                    anim.SetBool("Shooting", true);//handle shooting animation
                    audio.Play();//play shooting clip
                    StartCoroutine(Burst());//controlled burst of projectiles
                    canFire = false;//disable firing
                }
            }
        }
    }

    IEnumerator Burst()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerCtrl.IsFacingRight)
                // ... instantiate the projectile facing right and set it's velocity to the right. 
                ShootRight();
            else
                // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                ShootLeft();
            yield return new WaitForSeconds(.05f);//delay time between each projectile in burst being fired
        }
        anim.SetBool("Shooting", false);
        StartCoroutine(BurstDelay());//delay the ability to shoot another burst - directly after the previous is done
    }

    IEnumerator BurstDelay()
    {
        yield return new WaitForSeconds(burstDelay);
        canFire = true;//enable firing
    }

    public void ShootLeft()
    {
        Rigidbody2D bulletInstance = Instantiate(Bullet, bulletSpawnLoc.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
        bulletInstance.gameObject.GetComponent<Projectile>().whoShotMe = playerCtrl;
        Vector3 theScale = bulletInstance.transform.localScale;
        theScale.x *= -1;
        bulletInstance.transform.localScale = theScale;
        bulletInstance.velocity = new Vector2(-speed, 0);
        Destroy(bulletInstance.gameObject, 2);
    }

    public void ShootRight()
    {
        Rigidbody2D bulletInstance = Instantiate(Bullet, bulletSpawnLoc.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
        bulletInstance.gameObject.GetComponent<Projectile>().whoShotMe = playerCtrl;
        Vector3 theScale = bulletInstance.transform.localScale;
        theScale.x *= -1;
        bulletInstance.transform.localScale = theScale;
        bulletInstance.velocity = new Vector2(speed, 0);
        Destroy(bulletInstance.gameObject, 2);
    }
}
