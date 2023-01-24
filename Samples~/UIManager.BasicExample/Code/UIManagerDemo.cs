using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.ui.example
{
  public class UIManagerDemo : MonoBehaviour
  {
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField] private UIScreen WelcomePrefab;
    [SerializeField] private UIScreen SelectionPrefab;
    [SerializeField] private UIScreen InfiniteModalPrefab;


    private IEnumerator Start()
    {
      // give unity a sec to catch up so we can see animations effectively.
      yield return new WaitForSeconds(1.0f);
      
      //create an instance of the welcome screen.
      var welcomeScreen = _uiManager.ShowPrefab(WelcomePrefab);
      // wait for it to close (this causes the click shield to hide, then show, which is intended.
      yield return welcomeScreen.WaitForClose();
      // we're done with it, let's go ahead and clean it up. We always need to manually despawn modals since the modal
      // contains information about the user's interaction.
      welcomeScreen.Despawn();
      
      var selectionScreen = _uiManager.ShowPrefab(SelectionPrefab);
      while (true)
      {
        yield return selectionScreen.WaitForResult();
        if (selectionScreen.Result == 0)
        {
          var subScreen = _uiManager.ShowConfirmCancel("Here is a title", "This is the message.");
          yield return subScreen.WaitForResult();
          var confirm =_uiManager.ShowConfirm(null, $"Your Choice was {subScreen.Result}");
          subScreen.Despawn();
          yield return confirm.WaitForClose();
          confirm.Despawn();
          
        }

        if (selectionScreen.Result == -1)
        {
          yield return ShowInfiniteModal();
        }
      }
    }

    IEnumerator ShowInfiniteModal()
    {
      var modal = _uiManager.ShowPrefab(InfiniteModalPrefab);
      
      //while()
      yield return modal.WaitForResult();
      if (modal.Result == 0)
      {
        //pop another one onto the stack.
        yield return ShowInfiniteModal();
      }
      else
      {
        modal.Dismiss();
        modal.Despawn();
      }
    }
  }
}