using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterMoveAbility))]
[RequireComponent(typeof(CharacterRotateAbility))]
[RequireComponent(typeof(CharacterAttackAbility))]
public class Character : MonoBehaviour
{
    public Stat Stat;

    private void Start()
    {
        Stat.Init();

        UI_CharacterStat.Instance.MyCharacter = this;

    }
}
