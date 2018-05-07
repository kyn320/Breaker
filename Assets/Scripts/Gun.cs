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
        GameObject g = ObjectPoolManager.Instance.Get(bulletPrefab.name, tr.position, Quaternion.identity);
        g.GetComponent<Bullet>().SetBullet(player, bulletSpeed);
    }

}
