using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3Test : SkillBehaviour
{

    public float bulletSpeed;
    public GameObject bulletPrefab;

    public override void Use()
    {
        StartCoroutine(Action());
    }

    public override IEnumerator Action()
    {
        GameObject g = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);
        g.GetComponent<Bullet>().SetBullet(player, bulletSpeed, (int)(player.state.damage * skill.damagePercent));


        while (currentTime < skill.time)
        {

            yield return null;
        }
    }
}
