using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBehaviour
{
    public float bulletSpeed;
    public GameObject bulletPrefab;
    
    public override void Attack()
    {
        if (!isAttack)
            return;

        base.Attack();
        GameObject g = Instantiate(bulletPrefab, tr.position, tr.rotation);
        g.GetComponent<Bullet>().SetBullet(player, bulletSpeed);
    }

}
