using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Gun
{
    float msBetweenShots = 3.0f;

    float nextShootTime;

    public override void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + msBetweenShots / 100;
            Instantiate(projectile, transform.position, transform.rotation);
            Instantiate(projectile, transform.position, transform.rotation);
        }
    }
}
