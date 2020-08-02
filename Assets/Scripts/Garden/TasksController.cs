using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Things;
using UI;

namespace GameBase
{
  public class TasksController : MonoBehaviour
  {
    [SerializeField]
    SingleTask[] Tasks;

    private bool WrongWarnigDelay = false;

    public int Fails { get; private set; } = 0;
    public const int MaxFails = 5;

    #region Private Logic
    //---------------------------------------------------------------------------------------------------------------
    private bool HasMoreTasks => this.Tasks.FirstOrDefault(task => !task.Fulfilled) != null;

    //---------------------------------------------------------------------------------------------------------------
    private void WrongClick(Veggie veggie)
    {
      if (veggie == null)
      {
        return;
      }

      if (this.WrongWarnigDelay)
      {
        return;
      }
      this.Fails++;
      if (this.Fails >= MaxFails)
      {
        Game.Events.GameLost.Invoke();
        return;
      }
      if (Game.Settings.VibrationEnabled)
      {
        Handheld.Vibrate();
      }
      Game.AudioManager.PlaySound(AudioId.ClickNegative11);
      this.WrongWarnigDelay = true;
      Game.TimerManager.Start(0.3f, () => { this.WrongWarnigDelay = false; });
      PopedUIMsg.Pop(ObjectPoolName.WrongPop, veggie.transform.position);
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------
    public (int stars, int rewardPerStar) GetStarsAndReward()
    {
      (int stars, int rewardPerStar) result = (0, 100);
      if (this.Fails == 0)
      {
        result.stars = 3;
        return result;
      }

      if (this.Fails < 3)
      {
        result.stars = 2;
        return result;
      }

      if (this.Fails < 5)
      {
        result.stars = 1;
        return result;
      }

      return result;
    }

    //---------------------------------------------------------------------------------------------------------------
      public void CreateTasks()
    {
      List<SingleModel> models = Game.PlayRoot.Things.GetTasks(Tasks.Length);
      int counter = 0;

      foreach (SingleModel model in models)
      {
        this.Tasks[counter].AssignTask(model);
        counter++;
      }
    }

    //---------------------------------------------------------------------------------------------------------------
    public void VeggieClicked(Veggie veggie)
    {
      if (veggie == null)
      {
        Debug.LogError("Null veggie came trough Veggie clicked event");
        return;
      }

      if (veggie.IsVanished)
      {
        return;
      }

      SingleTask task = this.Tasks.FirstOrDefault(t => !t.Fulfilled && t.Model.IsEqual(veggie.CurrentModel));


      if (task == null)
      {
        this.WrongClick(veggie);
        return;
      }

      veggie.Vanish();
      task.MakeFulfilled();
      Game.Settings.SessionMoney = Game.Settings.SessionMoney + DefaultContent.DefaultTargetReward;

      if (Game.Settings.VibrationEnabled)
      {
        Handheld.Vibrate();
        Game.TimerManager.Start(0.3f, Handheld.Vibrate);
      }
      Game.AudioManager.PlaySound(AudioId.PositiveClick16);

      if (!this.HasMoreTasks)
      {
        Game.TimerManager.Start(0.35f, () => { Game.Events.GameWon.Invoke(); });
        
      }
    }

    #region MonoBehaviour
    //---------------------------------------------------------------------------------------------------------------
      void Start()
    {

    }
    #endregion

  }
}
