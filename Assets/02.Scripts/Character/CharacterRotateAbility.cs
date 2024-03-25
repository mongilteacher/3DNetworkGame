using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotateAbility : CharacterAbility
{
   // 목표: 마우스 이동에 따라 카메라와 플레이어를 회전하고 싶다.
   public Transform CameraRoot;
   
   private float _mx;
   private float _my;
   private void Update()
   {
      // 순서
      // 1. 마우스 입력 값을 받는다.
      float mouseX = Input.GetAxis("Mouse X");
      float mouseY = Input.GetAxis("Mouse Y");
      // 2. 회전 값을 마우스 입력에 따라 미리 누적한다.
      _mx += mouseX * Owner.Stat.RotationSpeed * Time.deltaTime;
      _my += mouseY * Owner.Stat.RotationSpeed * Time.deltaTime;
      _my = Mathf.Clamp(_my, -90f, 90f);
      // 3. 카메라(3인칭)와 캐릭터를 회전 방향으로 회전시킨다.
      transform.eulerAngles  = new Vector3(0, _mx, 0f);
      CameraRoot.localEulerAngles = new Vector3(-_my, 0, 0f);
   }
}
