using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Things;

namespace GameBase
{
  public class TasksController : MonoBehaviour
  {
    [SerializeField]
    SingleTask[] Tasks;

    #region Private Logic
    //---------------------------------------------------------------------------------------------------------------
    private bool HasMoreTasks => this.Tasks.FirstOrDefault(task => !task.Fulfilled) != null;

    //---------------------------------------------------------------------------------------------------------------
    private void WrongClick()
    {

    }
    #endregion

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

      SingleTask task = this.Tasks.FirstOrDefault(t => !t.Fulfilled && t.Model.IsEqual(veggie.CurrentModel));


      if (task == null)
      {
        this.WrongClick();
        return;
      }

      veggie.Vanish();
      task.MakeFulfilled();

      if (!this.HasMoreTasks)
      {
        Game.Events.SessionEnded.Invoke();
      }
    }

    #region MonoBehaviour
    //---------------------------------------------------------------------------------------------------------------
      void Start()
    {
      Game.PlayRoot.VeggieClicked.Listen(VeggieClicked);
    }
    #endregion

  }
}
