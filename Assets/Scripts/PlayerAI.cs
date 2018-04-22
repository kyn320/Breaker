using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : PlayerBehaviour
{
    public PlayerAIMoveState aiMoveState = PlayerAIMoveState.Idle;
    public PlayerAIActionState aiActionState = PlayerAIActionState.Idle;

    public float changeMoveStateTime = 0f;
    public float aiMoveStateTime = 0f;

    public float changeActionStateTime = 0f;
    public float aiActionStateTime = 0f;

    public Vector3 minMoveArea, maxMoveArea;
    public float wallMargin = 2f;

    [SerializeField]
    bool DEBUGMODE = false;
    Vector3[] moveAreaBox = new Vector3[6];

    protected override void Awake()
    {
        base.Awake();
        state.isAI = true;
        state.isInput = false;
    }

    void Start()
    {
        stateUpdate = StartCoroutine(StateUpdate());
    }


    Coroutine stateUpdate = null;

    IEnumerator StateUpdate()
    {

        while (true)
        {

            switch (aiMoveState)
            {
                case PlayerAIMoveState.Idle:
                    if (controller.focusDir.x < 0)
                        minMoveArea.x = InGameManager.instance.wall.transform.position.x + wallMargin;
                    else
                        maxMoveArea.x = InGameManager.instance.wall.transform.position.x - wallMargin;
                    break;
                case PlayerAIMoveState.Move:
                    if (controller.focusDir.x < 0)
                        minMoveArea.x = InGameManager.instance.wall.transform.position.x + wallMargin;
                    else
                        maxMoveArea.x = InGameManager.instance.wall.transform.position.x - wallMargin;
                    break;
                case PlayerAIMoveState.Doge:
                    break;
            }

            MoveAreaBoxing();

            aiMoveStateTime += Time.deltaTime;

            if (Physics2D.OverlapBox(moveAreaBox[4], moveAreaBox[5] * 2f, 0f, LayerMask.GetMask("Player")) == null)
            {
                ChangeMoveState(PlayerAIMoveState.Move);
            }


            switch (aiActionState)
            {
                case PlayerAIActionState.Idle:
                    break;
                case PlayerAIActionState.Attack:
                    controller.AttackKeyDown();
                    break;
                case PlayerAIActionState.Skill1:
                    controller.Hold();
                    if (controller.skillCount == 1)
                    {
                        controller.AttackKeyUp();
                    }
                    break;
                case PlayerAIActionState.Skill2:
                    controller.Hold();
                    if (controller.skillCount == 2)
                    {
                        controller.AttackKeyUp();
                    }
                    break;
                case PlayerAIActionState.Skill3:
                    controller.Hold();
                    if (controller.skillCount == 3)
                    {
                        controller.AttackKeyUp();
                    }
                    break;
            }

            aiActionStateTime += Time.deltaTime;

            if (aiMoveStateTime >= changeMoveStateTime)
                ChangeMoveState();

            if (aiActionStateTime >= changeActionStateTime)
                ChangeActionState();

            yield return null;
        }
    }

    void ChangeMoveState()
    {
        aiMoveState = (PlayerAIMoveState)Random.Range(0, 3);
        ResetAIStateTime(aiMoveState);
    }

    void ChangeMoveState(PlayerAIMoveState _aiState, float _changeTime = 0)
    {
        aiMoveState = _aiState;
        ResetAIStateTime(aiMoveState, _changeTime);
    }

    void ChangeActionState()
    {
        aiActionState = (PlayerAIActionState)Random.Range(1,5);
        ResetAIStateTime(aiActionState);
    }

    void ChangeActionState(PlayerAIActionState _aiState, float _changeTime = 0)
    {
        aiActionState = _aiState;
        ResetAIStateTime(aiActionState, _changeTime);
    }

    //움직임
    void ResetAIStateTime(PlayerAIMoveState _aiState, float _changeTime = 0)
    {
        aiMoveStateTime = 0;
        switch (_aiState)
        {
            case PlayerAIMoveState.Idle:

                controller.SetDir(0, 0);

                if (_changeTime > 0f)
                    changeMoveStateTime = _changeTime;
                else
                    changeMoveStateTime = 2f;
                break;
            case PlayerAIMoveState.Move:

                controller.SetTarget(new Vector2(Random.Range(minMoveArea.x, maxMoveArea.x), Random.Range(minMoveArea.y, maxMoveArea.y)));

                if (_changeTime > 0f)
                    changeMoveStateTime = _changeTime;
                else
                    changeMoveStateTime = 2f;
                break;
            case PlayerAIMoveState.Doge:
                if (_changeTime > 0f)
                    changeMoveStateTime = _changeTime;
                else
                    changeMoveStateTime = 0.1f;
                break;
        }
    }

    //공격
    void ResetAIStateTime(PlayerAIActionState _aiState, float _changeTime = 0)
    {
        aiActionStateTime = 0;
        switch (_aiState)
        {
            case PlayerAIActionState.Attack:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = 5f;
                break;
            case PlayerAIActionState.Skill1:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = skillList[0].skill.holdTime;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
            case PlayerAIActionState.Skill2:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = skillList[1].skill.holdTime;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
            case PlayerAIActionState.Skill3:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = skillList[2].skill.holdTime;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
        }
    }

    void MoveAreaBoxing()
    {
        moveAreaBox[0].x = minMoveArea.x;
        moveAreaBox[0].y = maxMoveArea.y;

        moveAreaBox[1].x = maxMoveArea.x;
        moveAreaBox[1].y = maxMoveArea.y;

        moveAreaBox[2].x = maxMoveArea.x;
        moveAreaBox[2].y = minMoveArea.y;

        moveAreaBox[3].x = minMoveArea.x;
        moveAreaBox[3].y = minMoveArea.y;

        moveAreaBox[4].x = (minMoveArea.x + maxMoveArea.x);
        moveAreaBox[4].y = (minMoveArea.y + maxMoveArea.y);

        moveAreaBox[4] *= 0.5f;

        moveAreaBox[5].x = Mathf.Abs(maxMoveArea.x - minMoveArea.x);
        moveAreaBox[5].y = Mathf.Abs(maxMoveArea.y - minMoveArea.y);
        moveAreaBox[5] *= 0.5f;

    }

    void OnDrawGizmos()
    {
        if (!DEBUGMODE)
            return;


        Gizmos.color = new Color32(125, 125, 125, 122);
        Gizmos.DrawCube(moveAreaBox[4] + Vector3.forward * 6f, moveAreaBox[5] * 2f);

        Gizmos.color = Color.blue;

        for (int i = 0; i < 4; ++i)
        {
            Gizmos.DrawLine(moveAreaBox[i] + Vector3.forward * 5f, moveAreaBox[i + 1 > 3 ? 0 : (i + 1)] + Vector3.forward * 5f);
        }

    }

}

public enum PlayerAIMoveState
{
    Idle,
    Move,
    Doge
}

public enum PlayerAIActionState
{
    Idle,
    Attack,
    Skill1,
    Skill2,
    Skill3
}