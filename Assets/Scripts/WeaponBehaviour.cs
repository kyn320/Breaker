using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{

    protected PlayerBehaviour player;

    protected Transform tr;

    public WeaponType type;

    public float delay;
    public float attackDelay;

    public bool isAttack = false;

    public GameObject attackEffect;
    public GameObject hitEffect;

    protected virtual void Awake()
    {
        tr = GetComponent<Transform>();
    }

    public void SetPlayer(PlayerBehaviour _player)
    {
        player = _player;
    }

    protected virtual void Update()
    {
        if (!isAttack) {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                isAttack = true;
            }
        }
    }

    public virtual void Attack()
    {
        isAttack = false;
        delay = attackDelay;
    }

    public virtual void OnCollisionEnter2D(Collision2D _col)
    {
        if (isAttack && _col.gameObject.CompareTag("Wall"))
        {
            ObjectPoolManager.Instance.Get(hitEffect.name, _col.contacts[0].point, Quaternion.identity);
            _col.gameObject.GetComponent<WallBehaviour>().Damage(player.state.damage,player.controller.focusDir);
        }

    }

    public virtual void OnCollisionExit2D(Collision2D _col)
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D _col) {
        if (isAttack &&  _col.gameObject.CompareTag("Wall"))
        {
            ObjectPoolManager.Instance.Get(hitEffect.name, _col.transform.position, Quaternion.identity);
            _col.gameObject.GetComponent<WallBehaviour>().Damage(player.state.damage, player.controller.focusDir);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D _col)
    {

    }

}

public enum WeaponType
{
    Gun,
    Bullet
}