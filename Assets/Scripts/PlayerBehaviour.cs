using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public PlayerState state;
    [HideInInspector]
    public PlayerController controller;
    
    public WeaponBehaviour weapon;

    protected virtual void Awake()
    {
        state = GetComponent<PlayerState>();
        controller = GetComponent<PlayerController>();
        weapon.SetPlayer(this);
    }

    public void Attack() {
        weapon.Attack();
    }


}
