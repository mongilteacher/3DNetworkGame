using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomList : MonoBehaviourPunCallbacks
{
    public List<UI_Room> UIRooms;

    public void Start()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (UI_Room roomUI in UIRooms)
        {
            roomUI.gameObject.SetActive(false);
        }
    }
    
    // 룸(방)의 정보가 변경(추가/수정/삭제)되었을 때 호출되는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Clear();

        List<RoomInfo> liveRoomList = roomList.FindAll(r => r.RemovedFromList == false);
        int roomCount = liveRoomList.Count;
        for (int i = 0; i < roomCount; ++i)
        {
            UIRooms[i].Set(liveRoomList[i]);
            UIRooms[i].gameObject.SetActive(true);
        }
    }
}
