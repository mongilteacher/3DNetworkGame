using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvasAbility : CharacterAbility
{
    public Canvas MyCanvas;
    public Text NicknameTextUI;

    private void Start()
    {
        NicknameTextUI.text = _owner.PhotonView.Controller.NickName;
    }

    private void Update()
    {
        // Todo. 빌보드 구현
    }
}
