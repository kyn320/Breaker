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

    public List<MoveArea> moveArea;
    private int moveAreaIndex = 0;
    private BoxCollider2D moveAreaCollider;
    public int belongAreaID = 0;
    public float wallMargin = 2f;

    public float moveAreaMarginUp;
    public float moveAreaMarginDown;
    public float moveAreaMarginLeft;
    public float moveAreaMarginRight;

    public int focusDir = 0;

    public Vector2 minMoveArea, maxMoveArea;
    public Vector2 moveAreaCenter, moveAreaSize;


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
                    else
                    {
                        maxMoveArea.x = manager.wallTransform.position.x - wallMargin;
                        focusDir = 1;
                    }

                    break;
                case PlayerAIMoveState.Doge:
                    break;
                case PlayerAIMoveState.Jump:
                    if (!state.isJump)
                    {
                        int checkAreaArrive = manager.FindContainID(tr);
                        belongAreaID = checkAreaArrive != -1 ? (checkAreaArrive + 1) : belongAreaID;
                        ChangeMoveState(PlayerAIMoveState.Move);
                    }
                    break;
            }

            if (moveArea.Count > 0
                && !manager.IsContainPointInArea((moveArea[moveAreaIndex].GetOrignAreaID() - 1), tr))
            {
                ChangeMoveState(PlayerAIMoveState.Move);
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

    public void EnterMoveArea()
    {
        moveArea = manager.GetMoveAreaWithID(belongAreaID);
        for (int i = 0; i < moveArea.Count; ++i)
        {
            moveArea[i].AddAI(this);
        }
    }

    public void ExitMoveArea()
    {
        for (int i = 0; i < moveArea.Count; ++i)
        {
            moveArea[i].SubAI(this);
        }
    }

    public Vector2 GetRandomPos()
    {
        ExitMoveArea();

        EnterMoveArea();

        if (moveArea.Count < 1)
        {
            print(gameObject.name+" Not Found ID" + belongAreaID);
            return Vector2.zero;
        }

        moveAreaIndex = Random.Range(0, moveArea.Count);

        moveAreaCollider = moveArea[moveAreaIndex].GetBoxCollider();
        belongAreaID = moveArea[moveAreaIndex].areaID;

        Vector2 randPos;

        if (focusDir < 0)
        {
            randPos = new Vector2(Random.Range(minMoveArea.x + moveAreaMarginLeft,
                                                    moveAreaCollider.bounds.max.x - moveAreaMarginRight),
                                        Random.Range(moveAreaCollider.bounds.min.y + moveAreaMarginDown,
                                                     moveAreaCollider.bounds.max.y - moveAreaMarginUp));
        }
        else
        {
            randPos = new Vector2(Random.Range(moveAreaCollider.bounds.min.x + moveAreaMarginLeft,
                                                    maxMoveArea.x - moveAreaMarginRight),
                                        Random.Range(moveAreaCollider.bounds.min.y + moveAreaMarginDown,
                                                     moveAreaCollider.bounds.max.y - moveAreaMarginUp));
        }

        return randPos;
    }

    public void ChangeAreaID(int _areaID)
    {
        belongAreaID = _areaID;
    }

    void OnDrawGizmos()
    {
        if (!DEBUGMODE || moveAreaCollider == null)
            return;

        Gizmos.color = Color.green;

        if (focusDir < 0)
        {
            moveAreaCenter.x = ((minMoveArea.x + moveAreaMarginLeft) + (moveAreaCollider.bounds.max.x - moveAreaMarginRight)) * 0.5f;
            moveAreaCenter.y = ((moveAreaCollider.bounds.min.y + moveAreaMarginDown) + (moveAreaCollider.bounds.max.y - moveAreaMarginUp)) * 0.5f;

            moveAreaSize.x = Mathf.Abs((moveAreaCollider.bounds.max.x - moveAreaMarginRight) - (minMoveArea.x + moveAreaMarginLeft));
            moveAreaSize.y = Mathf.Abs((moveAreaCollider.bounds.max.y - moveAreaMarginUp) - (moveAreaCollider.bounds.min.y + moveAreaMarginDown));
        }
        else
        {
            moveAreaCenter.x = ((moveAreaCollider.bounds.min.x + moveAreaMarginLeft) + (maxMoveArea.x - moveAreaMarginRight)) * 0.5f;
            moveAreaCenter.y = ((moveAreaCollider.bounds.min.y + moveAreaMarginDown) + (moveAreaCollider.bounds.max.y - moveAreaMarginUp)) * 0.5f;

            moveAreaSize.x = Mathf.Abs((maxMoveArea.x - moveAreaMarginRight) - (moveAreaCollider.bounds.min.x + moveAreaMarginLeft));
            moveAreaSize.y = Mathf.Abs((moveAreaCollider.bounds.max.y - moveAreaMarginUp) - (moveAreaCollider.bounds.min.y + moveAreaMarginDown));
        }

        Gizmos.DrawCube((Vector3)moveAreaCenter + Vector3.forward * 5f, moveAreaSize);
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
