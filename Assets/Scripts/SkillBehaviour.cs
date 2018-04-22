using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviour : MonoBehaviour
{
    public PlayerBehaviour player;
    public Skill skill;
    public float currentTime;

    public void SetPlayer(PlayerBehaviour _player)
    {
        player = _player;
    }

    public virtual void Use()
    {

    }

    public bool CheckUse(float _holdTime)
    {
        return _holdTime >= skill.holdTime;

    }


    public virtual IEnumerator Action()
    {

        yield return null;

    }

}
