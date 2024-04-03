using System;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerRankingSlot : MonoBehaviour
{
    public Text RankingTextUI;
    public Text NicknameTextUI;
    public Text KillCountTextUI;
    public Text ScoreTextUI;

    public void Set(Player player)
    {
        RankingTextUI.text = "1";
        NicknameTextUI.text = player.NickName;
        KillCountTextUI.text = "10";
        ScoreTextUI.text = "10000";
    }
    
}