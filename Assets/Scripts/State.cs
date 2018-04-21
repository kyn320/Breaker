using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태와 스텟을 정의하고 이용하는 컴포넌트
/// </summary>
public class State : MonoBehaviour
{
    public delegate void ChangeDel();

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int hp;
    /// <summary>
    /// 최대 체력
    /// </summary>
    public int maxHp;

    /// <summary>
    /// 현재 마나
    /// </summary>
    public int mp;
    /// <summary>
    /// 최대 마나
    /// </summary>
    public int maxMp;

    /// <summary>
    /// 공격력
    /// </summary>
    public int atk;
    /// <summary>
    /// 방어력
    /// </summary>
    public int def;
    /// <summary>
    /// 생명력
    /// </summary>
    public int lif;
    /// <summary>
    /// 행운
    /// </summary>
    public float luk;

    /// <summary>
    /// 데미지
    /// </summary>
    public int damage;

    /// <summary>
    /// 현재 슈퍼아머
    /// </summary>
    public int superAmor;
    /// <summary>
    /// 최대 슈퍼아머
    /// </summary>
    public int maxSuperAmor;

    /// <summary>
    /// 크리티컬 확률
    /// </summary>
    public float criticalPercent;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed;
    public ChangeDel speedChange;
    

    public virtual void AddHP(int _heal)
    {
        hp = Mathf.Clamp(hp + _heal, 0, maxHp);
    }

    public virtual void SubHP(int _damage)
    {
        hp = Mathf.Clamp(hp + _damage, 0, maxHp);
    }

    public virtual void AddMP(int _heal)
    {
        mp = Mathf.Clamp(mp + _heal, 0, maxMp);
    }

    public virtual void SubMP(int _cost)
    {
        mp = Mathf.Clamp(mp + _cost, 0, maxMp);
    }

    public virtual void SetAtk(int _atk)
    {
        atk = _atk;
        damage = _atk * 10;
    }

    public virtual void SetDef(int _def)
    {
        def = _def;
        maxSuperAmor = def * 10;
    }

    public virtual void SetLif(int _lif)
    {
        lif = _lif;
        maxHp = lif * 10;
    }

    public virtual void SetLuk(int _luk)
    {
        luk = _luk;
        criticalPercent = luk * 0.001f + 0.05f; 
    }

    public virtual void SetMoveSpeed(float _speed) {
        moveSpeed = _speed;
        speedChange();
    }


}
