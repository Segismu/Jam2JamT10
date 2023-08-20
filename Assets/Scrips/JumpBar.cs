using System;
using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
   [SerializeField] Image fillImage;
   [SerializeField] CatConfig catConfig;

   float fillrate;
   float maxFill;

   float currentValue;

   public void Awake()
   {
        fillImage.fillAmount = 0;

        maxFill = catConfig.maxJumpForce - catConfig.initialJumpForce;
        fillrate = catConfig.jumpChargeRate;
   }

   void Update()
   {
      
      if (PlayerController.State == CatState.PreparingJump)
      {
         currentValue += Time.deltaTime * fillrate/maxFill;
         fillImage.fillAmount = Mathf.Clamp(currentValue, 0f, 1);
      }
      else
      {
         currentValue = 0;
         fillImage.fillAmount = 0;
      }
   }
}
