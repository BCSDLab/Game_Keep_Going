using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    float msBetweenShots = 3.0f;

    float nextShootTime;

    public override void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + msBetweenShots / 1000;
            for (int i = 0; i < 5; i++)
            {
                Instantiate(projectile, transform.position, transform.rotation*Quaternion.Euler(0, Random.Range(-20, 20), 0));
            }
        }
    }
}
