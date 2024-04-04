using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMotionAbility : CharacterAbility
{
    private void Update()
    {
        if (_owner.State == State.Death || !_owner.PhotonView.IsMine)
        {
            return;
        }

        for (int i = 49; i <= 57; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                _owner.PhotonView.RPC(nameof(PlayMotion), RpcTarget.All, i - 49 + 1);
            }
        }
    }

    [PunRPC]
    private void PlayMotion(int number)
    {
        GetComponent<Animator>().SetTrigger($"Motion{number}");
    }
}
