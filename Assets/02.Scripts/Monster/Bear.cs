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
        Damaged,
        Death
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
        if (_targetCharacter == null)
        {
            return;
        }
        Vector3 targetPosition = _targetCharacter.transform.position;
        Vector3 myPosition    = transform.position;
        if (Vector3.Distance(targetPosition, myPosition) <= TraceDetectRange)
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
        Vector3 myPosition = transform.position;
        if (_targetCharacter != null)
        {
            Vector3 targetPosition = _targetCharacter.transform.position;

            if (Vector3.Distance(targetPosition, myPosition) <= TraceDetectRange)
            {
                _state = BearState.Trace;
                MyAnimatior.Play("Run");
                Debug.Log("Patrol -> Trace");
            }
        }
       
        
        // IF [패트롤 구역]에 도착하면 (복귀 상태로 전이)
        if (Vector3.Distance(PatrolDestination.position, myPosition) <= 0.1f)
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
}










