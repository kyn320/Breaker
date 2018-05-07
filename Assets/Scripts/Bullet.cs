using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBehaviour
{
    Vector2 dir;
    Rigidbody2D ri;
    public float speed;
    public int damage;

    protected override void Awake()
    {
        base.Awake();
        ri = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        ri.velocity = dir * speed;
    }

    public void SetBullet(PlayerBehaviour _player, float _speed, int _damage = 0)
    {
        SetPlayer(_player);
        dir = player.controller.focusDir;
        speed = _speed;
        if (_damage == 0)
            damage = player.state.damage;
        else
            damage = _damage;
    }


    public override void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Wall"))
        {
            ObjectPoolManager.Instance.Get(hitEffect.name, tr.position, Quaternion.identity);
            _col.gameObject.GetComponent<WallBehaviour>().Damage(damage, player.controller.focusDir);
            ri.velocity = Vector2.zero;
            SetPlayer(null);
            ObjectPoolManager.Instance.Free(this.gameObject);
        }
    }


}
