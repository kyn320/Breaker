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

    public Vector2 minMoveArea, maxMoveArea;

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
                        minMoveArea.x = InGameManager.instance.wall.transform.position.x + 1f;
                    else
                        maxMoveArea.x = InGameManager.instance.wall.transform.position.x - 1f;
                    break;
                case PlayerAIMoveState.Move:
                    if (controller.focusDir.x < 0)
                        minMoveArea.x = InGameManager.instance.wall.transform.position.x + 1f;
                    else
                        maxMoveArea.x = InGameManager.instance.wall.transform.position.x - 1f;
                    break;
                case PlayerAIMoveState.Doge:
                    break;
            }

            aiMoveStateTime += Time.deltaTime;

            switch (aiActionState)
            {
                case PlayerAIActionState.Idle:
                    break;
                case PlayerAIActionState.Attack:
                    Attack();
                    break;
                case PlayerAIActionState.Skill1:
                    break;
                case PlayerAIActionState.Skill2:
                    break;
                case PlayerAIActionState.Skill3:
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
        aiActionState = (PlayerAIActionState)Random.Range(0, 5);
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

                controller.SetTarget(new Vector2(Random.Range(minMoveArea.x, maxMoveArea.y), Random.Range(minMoveArea.y, maxMoveArea.y)));

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
                    changeActionStateTime = 0.1f;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
            case PlayerAIActionState.Skill2:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = 0.1f;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
            case PlayerAIActionState.Skill3:
                if (_changeTime > 0f)
                    changeActionStateTime = _changeTime;
                else
                    changeActionStateTime = 0.1f;

                ChangeMoveState(PlayerAIMoveState.Idle, changeActionStateTime);
                break;
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