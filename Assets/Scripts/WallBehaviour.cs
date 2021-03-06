﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{

    Transform tr;
    Rigidbody2D ri;

    protected WallState state;


    void Awake()
    {
        tr = GetComponent<Transform>();
        ri = GetComponent<Rigidbody2D>();

        state = GetComponent<WallState>();
    }

    public void Damage(int _damage, Vector3 _dir)
    {
        state.SubHP(_damage);
        ri.AddForce(_dir * _damage * 0.5f, ForceMode2D.Impulse);

        if (state.hp < 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _col.gameObject.GetComponent<PlayerBehaviour>().KnockBack(state.knockBackPower);
        }
    }
}
