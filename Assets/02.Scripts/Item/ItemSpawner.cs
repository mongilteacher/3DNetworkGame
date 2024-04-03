using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // 목표: 일정 시간마다 아이템을 랜덤한 개수만큼 생성해서 흩뿌리고 싶다.
    // 필요 속성
    // - 시간 (일정 시간, 현재 시간, 랜덤 최소/최대 시간)
    private float _currentTime;
    private float _createTime;
    public float MinCreateTime = 10;
    public float MaxCreateTime = 50;
    // - 랜덤 개수 (최소/최대 개수, 확정 개수)
    private float _createCount;
    public int MinCreateCount = 10;
    public int MaxCreateCount = 30;
    
    private void Start()
    {
        _createTime = 1f;
    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        // 구현 순서
        // 1. 시간이 흐르다가
        _currentTime += Time.deltaTime;
        // 2. 생성할 시간이 되면
        if (_currentTime >= _createTime)
        {
            // 3. 랜덤한 개수를 정하고
            _createCount = Random.Range(MinCreateCount, MaxCreateCount);
            // 4. 랜덤한 근처 위치에 생성한다.
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            randomPosition += transform.position;
            ItemObjectFactory.Instance.RequestCreate(ItemType.ScoreStone, randomPosition);
            // 5. 생성할 시간을 다시 랜덤...
            _createTime = Random.Range(MinCreateTime, MaxCreateTime);
        }
      
    }
    
}