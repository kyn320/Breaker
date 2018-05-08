using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrain : MonoBehaviour
{
    public Vector2[] originPos, moveRange;
    public List<Transform> trainList;

    [Header("-1 = 왼쪽 | 0  = 센터 | 1 = 오른쪽")]
    public int[] trainStateXList, trainStateYList;

    public float timeChangeDelay = 15f;
    public float moveSpeed = 1f;

    void Start()
    {
        StartCoroutine(UpdateTrainState());
    }

    IEnumerator UpdateTrainState()
    {
        while (true)
        {
            for (int i = 0; i < trainList.Count; ++i)
            {
                trainStateXList[i] = Random.Range(-2, 2);
                trainStateYList[i] = Random.Range(-2, 2);
            }

            yield return new WaitForSeconds(timeChangeDelay);
        }
    }

    void Update()
    {
        for (int i = 0; i < trainList.Count; ++i)
        {
            trainList[i].position = Vector2.Lerp(trainList[i].position,
                new Vector2(originPos[i].x + moveRange[i].x * trainStateXList[i],
                originPos[i].y + moveRange[i].y * trainStateYList[i]),
                Time.deltaTime * moveSpeed);
        }
    }

}
