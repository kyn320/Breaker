using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArea : MonoBehaviour
{
    private int originAreaID;
    public int areaID;

    BoxCollider2D boxCollider;

    public List<MoveArea> compositeList;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("MoveArea"))
        {
            MoveArea attachArea = _col.GetComponent<MoveArea>();
            int compositeAreaID = Mathf.Min(areaID, attachArea.areaID);
            compositeList.Add(attachArea);
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
        }
    }

}
