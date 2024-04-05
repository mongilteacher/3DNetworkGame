using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    // 곰 상태 상수(열거형)
    public enum BearState
    {
        Idle ,
        Patrol,
        Trace,
        Return,
        Attack,
    }

    private BearState _state = BearState.Idle;

    public Animator MyAnimatior;
    public NavMeshAgent Agent;

    private List<Character> _characterList = new List<Character>();
    public SphereCollider CharacterDetectCollider;
    private Character _targetCharacter;

    public Stat Stat;
    
    // [Idle]
    public float TraceDetectRange = 5f;
    public float IdleMaxTime      = 5f;
    private float _idleTime       = 0f;
    
    // [Patrol]
    public Transform PatrolDestination;
    
    // [Return]
    private Vector3 _startPosition;
    
    // [Attack]
    public float AttackDistance = 2.5f;
    private float _attackTimer = 0f;


    private void Start()
    {
        Agent.speed = Stat.MoveSpeed;
        
        _startPosition = transform.position;

        CharacterDetectCollider.radius = TraceDetectRange;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Character character = col.GetComponent<Character>();
            if (!_characterList.Contains(character))
            {
                Debug.Log("새로운 인간을 찾았다!");
                _characterList.Add(character);
            }
        }
    }
    
    
    // 매 프레임마다 해당 상태별로 정해진 행동을 한다.
    private void Update()
    {
        // 조기 반환
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        switch (_state)
        {
            case BearState.Idle:
            {
                Idle();
                break;
            }

            case BearState.Patrol:
            {
                Patrol();
                break;
            }

            case BearState.Return:
            {
                Return();
                break;
            }

            case BearState.Attack:
            {
                Attack();
                break;
            }
        }
    }

    private void Idle()
    {
        // 그러다가.. [대기 시간]이 너무 많으면 (정찰 상태로 전이)
        _idleTime += Time.deltaTime;
        if (_idleTime >= IdleMaxTime)
        {
            _idleTime = 0f;
            _state = BearState.Patrol;
            MyAnimatior.Play("Run");
            Debug.Log("Idle -> Patrol");
        }
        
        // 그러다가.. [플레이어]가 [감지 범위]안에 들어오면 플레이어 (추적 상태로 전이)
        _targetCharacter = FindTarget(TraceDetectRange);
        if (_targetCharacter != null)
        {
            _state = BearState.Trace;
            MyAnimatior.Play("Run");
            Debug.Log("Idle -> Trace");
        }
    }

    private void Patrol()
    {
        if (PatrolDestination == null)
        {
            PatrolDestination = GameObject.Find("Patrol").transform;
        }
        
        // [패트롤 구역]까지 간다.
        Agent.destination = PatrolDestination.position;
        Agent.stoppingDistance = 0f;
        
        // IF [플레이어]가 [감지 범위]안에 들어오면 플레이어 (추적 상태로 전이)
        _targetCharacter = FindTarget(TraceDetectRange);
        if (_targetCharacter != null)
        {
            _state = BearState.Trace;
            MyAnimatior.Play("Run");
            Debug.Log("Patrol -> Trace");
        }
        
        // IF [패트롤 구역]에 도착하면 (복귀 상태로 전이)
        if (!Agent.pathPending && Agent.remainingDistance <= 0.1f)
        {
            _state = BearState.Return;
            MyAnimatior.Play("Run");
            Debug.Log("Patrol -> Return");
        }
    }

    private void Return()
    {
        // [시작 위치]까지 간다.
        Agent.destination = _startPosition;
        Agent.stoppingDistance = 0f;
        
        if (!Agent.pathPending && Agent.remainingDistance <= 0.1f)
        {
            _state = BearState.Idle;
            MyAnimatior.Play("Idle");
            Debug.Log("Return -> Idle");
        }
    }

    private void Trace()
    {
        // 타겟이 게임에서 나가면 복귀
        if (_targetCharacter == null)
        {
            Debug.Log("Trace -> Return");
            _state = BearState.Return;
            return;
        }

        // 타겟이 죽거나 너무 멀어지면 복귀
        Agent.destination = _targetCharacter.transform.position;
        if (_targetCharacter.State == State.Death || GetDistance(_targetCharacter.transform) < TraceDetectRange)
        {
            Debug.Log("Trace -> Return");
            _state = BearState.Return;
            return;
        }
        
        // 타겟이 가까우면 공격 상태로 전이
        if (GetDistance(_targetCharacter.transform) <= AttackDistance)
        {
            Debug.Log("Trace -> Attack");
            MyAnimatior.Play("Idle");
            _state = BearState.Attack;
            return;
        }
    }

    private void Attack()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
        
        // 타겟이 게임에서 나가면 복귀
        if (_targetCharacter == null)
        {
            Debug.Log("Trace -> Return");
            Agent.isStopped = false;
            _startPosition = transform.position;
            _state = BearState.Idle;
            return;
        }

        // 타겟이 죽거나 공격 범위에서 벗어나면 복귀
        Agent.destination = _targetCharacter.transform.position;
        if (_targetCharacter.State == State.Death || GetDistance(_targetCharacter.transform) < AttackDistance)
        {
            Debug.Log("Trace -> Return");
            Agent.isStopped = false;
            _startPosition = transform.position;
            _state = BearState.Idle;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= Stat.AttackCoolTime)
        {
            _attackTimer = 0f;
            MyAnimatior.Play("Attack");
        }
    }

    
    
    // 나와의 거리가 distance보다 짧은 플레이어를 반환
    private Character FindTarget(float distance)
    {
        Vector3 myPosition = transform.position;
        foreach (Character character in _characterList)
        {
            if (Vector3.Distance(character.transform.position, myPosition) <= distance)
            {
                return character;
            }
        }

        return null;
    }
    
    
    private float GetDistance(Transform otherTransform)
    {
        return Vector3.Distance(transform.position, otherTransform.position);
    }
}










