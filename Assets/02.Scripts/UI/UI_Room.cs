using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class UI_Room : MonoBehaviour
{
    private RoomInfo _roomInfo;
    
    public Text RoomNameTextUI;
    public Text NicknameTextUI;
    public Text PlayerCountTextUI;

    public void Set(RoomInfo room)
    {
        _roomInfo = room;
        
        RoomNameTextUI.text    = room.Name;
        NicknameTextUI.text    = "방장 이름";
        PlayerCountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    // 룸 UI를 클릭했을때 호출되는 함수
    public void OnClickRoom()
    {
        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
    
}
