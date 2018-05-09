using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{

    public bool DEBUGMODE = false;

    public BoxCollider2D[] moveAreaCollider;

    public List<CompositeCollider2D> compositeMoveArea;

    public List<PlayerAI> aiList;

    public Transform wallTransform;

    void Awake()
    {
        PlayerAI.manager = this;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < moveAreaCollider.Length; ++i)
        {
            if (moveAreaCollider[i].composite != null && compositeMoveArea.Find(item => item == moveAreaCollider[i].composite) == null)
            {
                compositeMoveArea.Add(moveAreaCollider[i].composite);
            }
        }
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
            if (IsContainPointInArea(i, _tr))
                return i;

        }
        return -1;
    }

    public bool IsContainPointInArea(int _belongAreaID, Transform _tr)
    {
        return moveAreaCollider[_belongAreaID].bounds.Contains(_tr.position);
    }

    public bool IsContainBoundInArea(int _belongAreaID, Bounds _bounds)
    {
        return moveAreaCollider[_belongAreaID].bounds.Intersects(_bounds);
    }

    public BoxCollider2D GetMoveAreaColliderWithID(int _areaID)
    {
        return moveAreaCollider[_areaID];
    }

    public MoveArea GetMoveAreaWithID(int _areaID)
    {
        return moveAreaCollider[_areaID].GetComponent<MoveArea>();
    }


}
