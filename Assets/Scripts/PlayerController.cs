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

    public Vector2 moveDir, focusDir;

    float h, v;
    [SerializeField]
    Vector2 targetPos;

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
            Attack();

#elif UNITY_ANDROID || UNITY_IOS
        //joyStick
#endif

    }

    public void SetDir(float _h, float _v)
    {
        h = _h;
        v = _v;
    }

    public void SetTarget(Vector2 _target)
    {
        targetPos = _target;
        Vector2 dir = targetPos - (Vector2)tr.position;
        dir.Normalize();
        h = dir.x;
        v = dir.y;
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
    }

    public void Attack()
    {
        player.Attack();
    }

    public void Doge()
    {

    }

}
