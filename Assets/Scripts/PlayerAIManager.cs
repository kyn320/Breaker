using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{

    public bool DEBUGMODE = false;

    public BoxCollider2D[] moveAreaCollider;

    public List<PlayerAI> aiList;

    public Transform wallTransform;

    void Awake()
    {
        PlayerAI.manager = this;
    }

    void OnDrawGizmos()
    {
        if (!DEBUGMODE)
            return;

        Gizmos.color = Color.blue;

        for (int i = 0; i < moveAreaCollider.Length; ++i)
        {
            Gizmos.DrawWireCube(moveAreaCollider[i].bounds.center, moveAreaCollider[i].bounds.size);
        }

    }

    public int FindContainID(Transform _tr)
    {
        for (int i = 0; i < moveAreaCollider.Length; ++i)
        {
            if (IsContainArea(i, _tr))
                return i;
            
        }
        return -1;
    }

    public bool IsContainArea(int _belongAreaID, Transform _tr)
    {
        return moveAreaCollider[_belongAreaID].bounds.Contains(_tr.position);
    }

    public BoxCollider2D GetMoveAreaWithID(int _areaID)
    {
        return moveAreaCollider[_areaID];
    }



}
