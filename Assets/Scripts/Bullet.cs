using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBehaviour
{
    Vector2 dir;
    Rigidbody2D ri;
    public float speed;

    protected override void Awake()
    {
        base.Awake();
        ri = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        ri.velocity = dir * speed;
    }

    public void SetBullet(PlayerBehaviour _player, float _speed)
    {
        SetPlayer(_player);
        dir = player.controller.focusDir;
        speed = _speed;
    }

    public override void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Wall"))
        {
            Instantiate(hitEffect, tr.position, Quaternion.identity);
            _col.gameObject.GetComponent<WallBehaviour>().Damage(player.state.damage, player.controller.focusDir);
            Destroy(this.gameObject);
        }
    }


}
