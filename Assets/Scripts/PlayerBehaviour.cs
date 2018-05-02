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

    public Animator ani;

    [Header("애니메이션 이름")]
    [Header("0 = 대기 | 1 = 달리기 | 2 = 공격 | 3 = 스킬")]
    [SerializeField]
    string[] aniSet = new string[] { "Idle", "Run", "Attack", "Skill" };


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

    public void KnockBack(float _power)
    {
        controller.KnockBack(_power);
    }

    public void AnimationPlay(int _id)
    {
        ani.Play(aniSet[_id]);
    }


}
