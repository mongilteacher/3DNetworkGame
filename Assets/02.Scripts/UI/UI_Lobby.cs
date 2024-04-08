using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public InputField NicknameInputFieldUI;
    public InputField RoomIDInputFieldUI;

    // 방만들기 버튼을 눌렀을때 호출되는 함수
    public void OnClickMakeRoomButton()
    {
        string nickname = NicknameInputFieldUI.text;
        string roomID = RoomIDInputFieldUI.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomID))
        {
            Debug.Log("입력하세요.");
            return;
        }

        PhotonNetwork.NickName = nickname;
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;   // 입장 가능한 최대 플레이어 수
        roomOptions.IsVisible  = true; // 로비에서 방 목록에 노출할 것인가?
        roomOptions.IsOpen     = true; // 방에 다른 플레이어가 들어올 수 있는가?
        
        PhotonNetwork.JoinOrCreateRoom(roomID, roomOptions, TypedLobby.Default); // 방이 있다면 입장하고 없다면 만드는 것
    }
}
