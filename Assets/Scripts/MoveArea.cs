using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArea : MonoBehaviour
{
    private int originAreaID;
    public int areaID;

    BoxCollider2D boxCollider;

    public List<MoveArea> compositeList;
    public List<PlayerAI> inAreaAiList;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        originAreaID = areaID;
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("MoveArea"))
        {
            MoveArea attachArea = _col.GetComponent<MoveArea>();
            int compositeAreaID = Mathf.Min(areaID, attachArea.areaID);
            compositeList.Add(attachArea);
            areaID = compositeAreaID;
            UpdateAiAreaID();

        }
    }

    private void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.CompareTag("MoveArea"))
        {
            compositeList.Remove(_col.GetComponent<MoveArea>());

            int compositeAreaID = originAreaID;

            for (int i = 0; i < compositeList.Count; ++i)
            {
                compositeAreaID = Mathf.Min(originAreaID, compositeList[i].originAreaID);
            }

            areaID = compositeAreaID;
            UpdateAiAreaID();
        }
    }

    public BoxCollider2D GetBoxCollider() {
        return boxCollider;
    }

    public void AddAI(PlayerAI _ai) {
        inAreaAiList.Add(_ai);
    }

    public void SubAI(PlayerAI _ai) {
        inAreaAiList.Remove(_ai);
    }

    public void UpdateAiAreaID() {
        for (int i = 0; i < inAreaAiList.Count; ++i)
        {
            inAreaAiList[i].ChangeAreaID(areaID);
        }
    }

    public int GetOrignAreaID() {
        return originAreaID;
    }
}
