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

    public SkillBehaviour[] skillList;

    protected virtual void Awake()
    {
        state = GetComponent<PlayerState>();
        controller = GetComponent<PlayerController>();
        weapon.SetPlayer(this);
        for (int i = 0; i < skillList.Length; ++i)
        {
            skillList[i].SetPlayer(this);
        }
    }

    public void Attack()
    {
        weapon.Attack();
    }

    public void KnockBack(float _power) {
        controller.KnockBack(_power);
    }


}
