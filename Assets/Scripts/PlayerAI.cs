using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : PlayerBehaviour
{
    public static PlayerAIManager manager;

    private Transform tr;

    public PlayerAIMoveState aiMoveState = PlayerAIMoveState.Idle;
    public PlayerAIActionState aiActionState = PlayerAIActionState.Idle;

    public float changeMoveStateTime = 0f;
    public float aiMoveStateTime = 0f;

    public float changeActionStateTime = 0f;
    public float aiActionStateTime = 0f;

    private BoxCollider2D moveAreaCollider;
    public int belongAreaID = 0;
    public float wallMargin = 2f;
    public int focusDir = 0;

    public Vector2 minMoveArea, maxMoveArea;

    [SerializeField]
    bool DEBUGMODE = false;

    protected override void Awake()
    {
        base.Awake();
        state.isAI = true;
        state.isInput = false;
        tr = GetComponent<Transform>();
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
                case PlayerAIMoveState.Move:
                    if (controller.focusDir.x < 0)
                    {
                        minMoveArea.x = manager.wallTransform.position.x + wallMargin;
                        focusDir = -1;
                    }
                    else {
                        maxMoveArea.x = manager.wallTransform.position.x - wallMargin;
                        focusDir = 1;
                    }

                    if (!manager.IsContainArea(belongAreaID, tr))
                    {
                        ChangeMoveState(PlayerAIMoveState.Move);
                    }

                    break;
                case PlayerAIMoveState.Doge:
                    break;
                case PlayerAIMoveState.Jump:
                    if (!state.isJump)
                    {
                        int checkAreaArrive = manager.FindContainID(tr);
                        belongAreaID = checkAreaArrive != -1 ? checkAreaArrive : belongAreaID;
                        ChangeMoveState(PlayerAIMoveState.Move);
                    }
                    break;
            }

            aiMoveStateTime += Time.deltaTime;

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
        aiMoveState = (PlayerAIMoveState)Random.Range(0, 4);
        ResetAIStateTime(aiMoveState);
    }

    void ChangeMoveState(PlayerAIMoveState _aiState, float _changeTime = 0)
    {
        aiMoveState = _aiState;
        ResetAIStateTime(aiMoveState, _changeTime);
    }

    void ChangeActionState()
    {
        aiActionState = (PlayerAIActionState)Random.Range(1, 2);
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

                controller.SetTarget(GetRandomPos());

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

            case PlayerAIMoveState.Jump:
                controller.SetTarget(GetRandomPos());
                controller.Jump();
                changeMoveStateTime = 999f;
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

    public Vector2 GetRandomPos()
    {
        moveAreaCollider = manager.GetMoveAreaWithID(belongAreaID);

        Vector2 randPos;

        if (focusDir < 0)
        {
            randPos = new Vector2(Random.Range(minMoveArea.x,
                                                    moveAreaCollider.bounds.max.x),
                                        Random.Range(moveAreaCollider.bounds.min.y,
                                                     moveAreaCollider.bounds.max.y));
        }
        else {
            randPos = new Vector2(Random.Range(moveAreaCollider.bounds.min.x,
                                                    maxMoveArea.x),
                                        Random.Range(moveAreaCollider.bounds.min.y,
                                                     moveAreaCollider.bounds.max.y));
        }

        return randPos;
    }

}

public enum PlayerAIMoveState
{
    Idle,
    Move,
    Doge,
    Jump
}

public enum PlayerAIActionState
{
    Idle,
    Attack,
    Skill1,
    Skill2,
    Skill3
}