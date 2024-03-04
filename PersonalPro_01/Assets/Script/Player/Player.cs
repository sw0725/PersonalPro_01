using Cinemachine.Utility;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float sitMoveSpeed = 0.01f;
    public float rotateSpeedX = 1.0f;
    public float rotateSpeedY = 1.0f;
    public float jumpPower = 6.0f;
    public float jumpCoolTime = 1.0f;
    public Action OnDie;
    public Action OnShoot;

    Vector3 MoveDirection = Vector3.zero;   
    Vector3 RotateDirection = Vector3.zero;  
    float jumpTime = -1.0f;  
    bool isjumping = false;
    bool IsJumpAvailable => !isjumping && (jumpTime < 0.0f);
    bool isAlive = true;
    float currentSpeed = 0.0f;
    bool isCrouching = false;
    bool hideL = false;
    bool shootmode = true;

    readonly int isMovehash = Animator.StringToHash("IsMove");
    readonly int isUsehash = Animator.StringToHash("Use");
    readonly int isDiehash = Animator.StringToHash("IsDie");
    readonly int isJumphash = Animator.StringToHash("IsJump");
    readonly int isHideLhash = Animator.StringToHash("IsHideL");
    readonly int isHideRhash = Animator.StringToHash("IsHideR");
    readonly int isIShoothash = Animator.StringToHash("IShootMode");

    PlayerInputAction inputActions;
    Transform model;
    Transform camRoot;
    Animator anim;
    Animator Canim;
    Rigidbody rigid;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();

        model = transform.GetChild(0);
        camRoot = transform.GetChild(2);
        anim = model.GetChild(0).GetComponent<Animator>();
        Canim = GetComponent<Animator>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>(true);
        checker.onItemUse += (inter) => inter.Use();

        currentSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Use.performed += OnUse;
        //inputActions.Player.Look.performed += OnRotate;
        inputActions.Player.Lock.performed += OnRClick;
        //inputActions.Player.Shoot.performed += OnLClick;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Use.performed -= OnUse;
        //inputActions.Player.Look.performed -= OnRotate;
        inputActions.Player.Lock.performed -= OnRClick;
        //inputActions.Player.Shoot.performed -= OnLClick;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)     
    {
        SetMoveInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        Jump();
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        Canim.SetTrigger(isUsehash);
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        SetRotationInput(context.ReadValue<Vector2>());
    }

    private void OnRClick(InputAction.CallbackContext context)
    {
        if (shootmode)
        {
            inputActions.Player.Look.performed -= OnRotate;
            inputActions.Player.Shoot.performed += OnLClick;
        }
        else 
        {
            inputActions.Player.Look.performed += OnRotate;
            inputActions.Player.Shoot.performed -= OnLClick;
        }
        anim.SetBool(isIShoothash, shootmode);
        shootmode = !shootmode;
    }

    private void OnLClick(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }

    void SetMoveInput(Vector2 input, bool isMove)
    {
        MoveDirection.x = input.y;
        MoveDirection.z = input.x;
        if(!isCrouching)
            anim.SetBool(isMovehash, isMove);
    }
    void SetRotationInput(Vector2 input)
    {
        RotateDirection.y += input.x * rotateSpeedY;
        RotateDirection.x += input.y * rotateSpeedX;
    }

    void Move()
    {   //계속불리는 종류의 변수는 재활용하다가 피본다
        Vector3 move = MoveDirection.x * transform.forward + MoveDirection.z * transform.right;
        transform.position += Time.fixedDeltaTime * currentSpeed * move;
    }

    void Rotate() 
    {
        float yAngle = Time.fixedDeltaTime * RotateDirection.y;
        Quaternion yrotate = Quaternion.AngleAxis(yAngle, transform.up);
        transform.rotation = yrotate;
        float xAngle = Time.fixedDeltaTime * RotateDirection.x;
        xAngle = Mathf.Clamp(xAngle, -40.0f, 40.0f);
        camRoot.rotation = Quaternion.Euler(-xAngle, yAngle, 0);
    }

    void Jump()
    {
        if (IsJumpAvailable)
        {
            anim.SetTrigger(isJumphash);
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            jumpTime = jumpCoolTime;
            isjumping = true;
        }
    }

    public void Die()
    {
        if (isAlive)
        {
            anim.SetTrigger(isDiehash);
            inputActions.Player.Disable();

            rigid.constraints = RigidbodyConstraints.None;  
            Transform head = transform.GetChild(0);

            rigid.AddForceAtPosition(-transform.forward, head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.5f, ForceMode.Impulse);
            OnDie?.Invoke();

            isAlive = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Update()
    {
        jumpTime = -Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isjumping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CrouchL"))
        {
            hideL = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CrouchL"))
        {
            hideL = false;
        }
    }

    public void GotoSitMode() 
    {
        currentSpeed = sitMoveSpeed;
        isCrouching = true;

        if (hideL)
        {
            anim.SetBool(isHideLhash, isCrouching);
        }
        else 
        {
            anim.SetBool(isHideRhash, isCrouching);
        }
    }

    public void GoToStandMode()
    {
        currentSpeed = moveSpeed;
        isCrouching = false;

        anim.SetBool(isHideLhash, isCrouching);
        anim.SetBool(isHideRhash, isCrouching);
    }
}
