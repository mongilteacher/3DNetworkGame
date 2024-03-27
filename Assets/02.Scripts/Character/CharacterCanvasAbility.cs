using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvasAbility : CharacterAbility
{
    public Canvas MyCanvas;
    
    public Text NicknameTextUI;
    
    public Slider HealthSliderUI;
    public Slider StaminaSliderUI;
    
    private void Start()
    {
        NicknameTextUI.text = _owner.PhotonView.Controller.NickName;
    }

    private void Update()
    {
        MyCanvas.transform.forward = Camera.main.transform.forward;

        HealthSliderUI.value  = (float)_owner.Stat.Health / _owner.Stat.MaxHealth;
        StaminaSliderUI.value = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
