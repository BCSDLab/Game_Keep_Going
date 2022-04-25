using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float projectilesSpeed = 20;
    public LayerMask collisonMask;
    float projectileDamage = 1.0f;
    float projectileLifeTime = 3.0f;

    
    void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }
    void Update()
    {
        float moveDistance = projectilesSpeed * Time.deltaTime;
        CheckCollisons(moveDistance);
        transform.Translate(Vector3.forward * Time.deltaTime * projectilesSpeed);
    }
    void CheckCollisons(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisonMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }
    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if(damageableObject != null)
        {
            damageableObject.TakeHit(projectilesSpeed, hit);
        }
        print(hit.collider.gameObject.name);
        Destroy(gameObject);
    }
}
