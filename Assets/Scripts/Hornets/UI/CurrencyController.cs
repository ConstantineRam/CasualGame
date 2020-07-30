using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
  [SerializeField]
  private Text MyTextField;

  private int CurrentAmount;
  private bool TimerFlag = false;
  private const float MaxTimer = 0.01f;
  private const float MinTimer = 0.005f;
  private const float TimerStep = 0.001f;
  private const float ShakeMod = TimerStep * 2;
  private float CurrentTimer = MaxTimer;
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      this.CurrentAmount = Game.Settings.SoftCurrency;
    }
    else
    {
      this.CurrentAmount = Game.Settings.SessionMoney;
    }
    
    this.SetText(this.CurrentAmount); 
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Update()
  {
    if (TimerFlag)
    {
      return;
    }

    if (!this.IsTimeToUpdate)
    {
      this.ResetTimer();
      return;
    }

    this.TimerFlag = true;
    this.CurrentAmount++;
    this.SetText(this.CurrentAmount);
    this.MyTextField.transform.DOShakeScale(duration: (this.CurrentTimer - ShakeMod), strength: 0.2f, vibrato: 0, randomness: 10 ).SetAutoKill(); 
    Game.TimerManager.Start(this.CurrentTimer, ()=> { this.TimerFlag = false; });
    this.DecTimer();
  }
  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public void Push(int value)
  {
    if (value < 1)
    {
      Debug.LogError("Unexpected error. Push (Currency Controller) got invalid value " + value);
      return;
    }
    
    Game.Settings.SessionMoney = Game.Settings.SessionMoney + value;

  }

  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void SetText(int value)
  {
    this.MyTextField.text = value.ToString();
  }
  //---------------------------------------------------------------------------------------------------------------
  private void ResetTimer()
  {
    this.CurrentTimer = MaxTimer;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void DecTimer()
  {
    if (this.CurrentTimer <= MinTimer)
    {
      return;
    }

    this.CurrentTimer -= TimerStep;
  }
  //---------------------------------------------------------------------------------------------------------------
  private bool IsTimeToUpdate
  {
    get
    {
      if (CurrentAmount < Game.Settings.SessionMoney)
      {
        return true;
      }

      return false;
    }
  }
  #endregion
}
