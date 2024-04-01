using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{
    public static ItemObjectFactory Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void RequestCreate(ItemType type, Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Create(type, position);
        }
        else
        {
            photonView.RPC(nameof(Create), RpcTarget.MasterClient, type, position);
        }
    }
    
    [PunRPC]
    private void Create(ItemType type, Vector3 position)
    {
        Vector3 dropPos = position + new Vector3(0, 0.5f, 0f) + UnityEngine.Random.insideUnitSphere;
        PhotonNetwork.InstantiateRoomObject(type.ToString(), dropPos, Quaternion.identity);
    }

    public void RequestDelete(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(viewID);
        }
        else
        {
            photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewID);
        }
    }

    [PunRPC]
    private void Delete(int viewID)
    {
        GameObject objectToDelete = PhotonView.Find(viewID).gameObject;
        if (objectToDelete != null)
        {
            PhotonNetwork.Destroy(objectToDelete);
        }
    }
}
