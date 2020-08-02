using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace UI
{

  public class UpdatableIntString : BasicMono
  {
    [SerializeField]
    private Text MyTextField;
    [SerializeField]
    private bool ShakeOnPush = true;

    private int CurrentAmount = 0;
    private int PushedAmount = 0;
    private bool TimerFlag = false;
    [SerializeField]
    [Tooltip("The speed of the number update.")]
    [Range(MinTimer, MaxTimer)]
    private float UpdateSpeed = MaxTimer;
    private const float MaxTimer = 0.01f;
    private const float MinTimer = 0.003f;
    private const float TimerStep = 0.001f;
    private const float ShakeTime = 0.5f;
    private float CurrentTimer = MaxTimer;
    public bool Initialized { get; private set; } = false;
    //---------------------------------------------------------------------------------------------------------------
    public void Init(int Amount)
    {
      this.Initialized = true;
      this.CurrentAmount = Amount;
      this.SetText(this.CurrentAmount);
    }

    //---------------------------------------------------------------------------------------------------------------
    public void Push(int value)
    {
      if (!this.Initialized)
      {
        Debug.LogError(" Int String at "+ this.name + " wasn't initialized.");
        return;
      }
      if (value < 1)
      {
        Debug.LogError("Unexpected error. Push at " + this.name + "  got invalid value " + value);
        return;
      }

      if (this.ShakeOnPush)
      {
        this.MyTextField.transform.DOShakeScale(duration: ShakeTime, strength: 3f, vibrato: 0, randomness: 10).SetAutoKill();
      }
      this.PushedAmount += value;

    }
    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected override void OnAwake()
    {
      base.OnAwake();

      if (this.MyTextField == null)
      {
        this.MyTextField = this.GetComponent<Text>();
      }

    }

    //---------------------------------------------------------------------------------------------------------------
    protected override void OnUpdate()
    {
      base.OnUpdate();

      if (!this.Initialized)
      {
        return;
      }

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
      this.PushedAmount--;
      this.SetText(this.CurrentAmount);
      
      Game.TimerManager.Start(this.CurrentTimer, () => { this.TimerFlag = false; });
      this.DecTimer();
    }
    #endregion

    #region Private
    //---------------------------------------------------------------------------------------------------------------
    private void SetText(int value)
    {
      this.MyTextField.text = value.ToString();
    }

    //---------------------------------------------------------------------------------------------------------------
    private void ResetTimer()
    {
      this.CurrentTimer = this.UpdateSpeed;
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
    private bool IsTimeToUpdate => this.PushedAmount > 0;
    #endregion
  }
}