using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class BattleScene : MonoBehaviourPunCallbacks
{
    public static BattleScene Instance { get; private set; }

    public List<Transform> SpawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, SpawnPoints.Count);
        return SpawnPoints[randomIndex].position;
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        GameObject[] points = GameObject.FindGameObjectsWithTag("BearSpawnPoint");
        foreach (GameObject point in points)
        {
            PhotonNetwork.InstantiateRoomObject("Bear", point.transform.position, Quaternion.identity);
        }
    }
    
}
   
