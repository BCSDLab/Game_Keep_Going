using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile projectile;
    float msBetweenShots = 3.0f;

    float nextShootTime;

    public void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + msBetweenShots/1000;
            Projectile newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        }
    }
}
