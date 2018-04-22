using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform tr;
    Rigidbody2D ri;

    PlayerBehaviour player;

    public float orignMoveSpeed;
    public float addMoveSpeed;

    public Vector3 moveDir, focusDir;

    float h, v;


    public float skillHoldTime;
    public int skillCount = 0;

    [SerializeField]
    Vector3 targetPos;
    [SerializeField]
    bool targetMove = false;

    [SerializeField]
    bool DEBUGMODE = false;

    void Awake()
    {

        tr = GetComponent<Transform>();
        ri = GetComponent<Rigidbody2D>();

        player = GetComponent<PlayerBehaviour>();

    }

    void Start()
    {
        player.state.speedChange += SetMoveSpeed;
    }


    void Update()
    {
        InputUpdate();
        Move();

    }

    public void InputUpdate()
    {
        if (!player.state.isInput || player.state.isAI)
            return;

#if UNITY_STANDALONE
        //KeyBoard
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");


        if (Input.GetKey(KeyCode.Z))
        {
            AttackKeyDown();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            AttackKeyUp();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            Hold();
        }

#elif UNITY_ANDROID || UNITY_IOS
        //joyStick
#endif

    }

    public void Hold()
    {
        skillHoldTime += Time.deltaTime;
        int checkSkillCount = 0;
        for (int i = 0; i < player.skillList.Length; ++i)
        {
            if (player.skillList[i].CheckUse(skillHoldTime))
            {
                ++checkSkillCount;
            }
        }
        skillCount = checkSkillCount;
    }

    public void AttackKeyUp()
    {
        skillHoldTime = 0;
        if (skillCount > 0)
        {
            player.skillList[skillCount - 1].Use();
            skillCount = 0;
        }
    }

    public void AttackKeyDown()
    {
        Attack();
    }

    public void SetDir(float _h, float _v)
    {
        h = _h;
        v = _v;
    }

    public void SetTarget(Vector3 _target)
    {
        targetPos = _target;
        Vector3 dir = targetPos - tr.position;
        dir.Normalize();
        h = dir.x;
        v = dir.y;

        targetMove = true;
    }

    public void SetMoveSpeed()
    {
        orignMoveSpeed = player.state.moveSpeed;
    }

    public void Move()
    {
        if (!player.state.isMove)
            return;

        moveDir = new Vector2(h, v).normalized;

        ri.velocity = moveDir * (orignMoveSpeed + addMoveSpeed);

        if (targetMove && (targetPos - tr.position).sqrMagnitude <= 0.1f)
        {
            h = v = 0;
            moveDir = targetPos = Vector3.zero;
            targetMove = false;
        }
    }

    public void Attack()
    {
        player.Attack();
    }

    public void Doge()
    {

    }

    void OnDrawGizmos()
    {
        if (!DEBUGMODE)
            return;

        if (moveDir != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(tr.position + Vector3.forward * 3f, moveDir * 1f);
        }

        if (targetMove)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(targetPos + Vector3.forward * 3f, 0.1f);
        }
    }

}
