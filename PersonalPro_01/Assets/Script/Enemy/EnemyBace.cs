using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBace : RecycleObject
{
    public float moveSpeed = 1.0f;
    public int maxHp = 3;
    public int score = 10;

    Action<int> giveScore;
    GameManager manager;
    Transform player;
    NavMeshAgent agent;
    Animator animator;
    State state;

    int hp = 1;
    protected int HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp < 0.1)
            {
                hp = 0;
                OnDie();
            }
        }
    }

    enum State 
    {
        Idel,
        Run,
        Atteck
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        GameObject model = transform.GetChild(0).gameObject;
        animator = model.GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialized();
        manager = FindAnyObjectByType<GameManager>();
        player = GameManager.Instance.Player.transform;
        agent.destination = player.transform.position;
        giveScore += manager.ScoreUp;
        state = State.Idel;
        animator.SetBool("Attack", false);
        animator.SetBool("Run", false);
    }

    protected override void OnDisable()
    {
        giveScore -= manager.ScoreUp;
        giveScore = null;
        player = null;
        base.OnDisable();
    }

    void Update()
    {
        if (!GameManager.gameOver) 
        {
            if (state == State.Run)
            {
                OnMoveUpdate(Time.deltaTime);
            }
            else if (state == State.Atteck)
            {
                OnAtteckUpdate(Time.deltaTime);
            }
            else
            {
                OnIdleUpdate(Time.deltaTime);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
    }

    protected virtual void OnDie()
    {
        giveScore?.Invoke(score);
        gameObject.SetActive(false);
    }

    public virtual void OnDemage()
    {
        HP -= 1;
    }

    protected virtual void OnInitialized()
    {
        HP = maxHp;
    }

    protected virtual void OnMoveUpdate(float deltaTime)
    {
        float distace = Vector3.Distance(transform.position, player.position);
        if (distace < 2) 
        {
            state = State.Atteck;
            animator.SetBool("Attack", true);
        }
        agent.speed = moveSpeed;
        agent.destination = player.transform.position;
        //transform.Translate(deltaTime * moveSpeed * -transform.forward);
    }
    protected virtual void OnIdleUpdate(float deltaTime)
    {
        agent.speed = 0;
        if (player != null) 
        {
            state = State.Run;
            animator.SetBool("Run", true);
        }
        else 
        {
            player = GameManager.Instance.Player.transform;
        }
    }
    protected virtual void OnAtteckUpdate(float deltaTime)
    {
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 2) 
        {
            state = State.Run;
            animator.SetBool("Attack", false);
        }
    }
}
