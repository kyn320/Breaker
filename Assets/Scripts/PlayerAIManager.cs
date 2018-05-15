using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{
    public bool DEBUGMODE = false;

    public MoveArea[] moveAreaList;
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
        for (int i = 0; i < moveAreaList.Length; ++i)
        {
            if (IsContainPointInArea(i, _tr))
                return i;

        }
        return -1;
    }

    public bool IsContainPointInArea(int _belongAreaID, Transform _tr)
    {
        return moveAreaList[_belongAreaID].GetBoxCollider().bounds.Contains(_tr.position);
    }

    public bool IsContainBoundInArea(int _belongAreaID, Bounds _bounds)
    {
        return moveAreaList[_belongAreaID].GetBoxCollider().bounds.Intersects(_bounds);
    }

    public MoveArea GetMoveAreaWithIndex(int _index)
    {
        return moveAreaList[_index].GetComponent<MoveArea>();
    }

    public List<MoveArea> GetMoveAreaWithID(int _areaID)
    {
        List<MoveArea> list = new List<MoveArea>();

        for (int i = 0; i < moveAreaList.Length; ++i)
        {
            if (moveAreaList[i].areaID == _areaID)
                list.Add(moveAreaList[i]);
        }

        return list;
    }
    
}
