using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.ui.example
{
  public class UIManagerDemo : MonoBehaviour
  {
    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private ScreenManager _screenManager;

    [SerializeField] private UIScreen WelcomePrefab;
    [SerializeField] private UIScreen WelcomeScreen1Prefab;
    [SerializeField] private UIScreen WelcomeScreen2Prefab;
    [SerializeField] private UIScreen SelectionPrefab;
    [SerializeField] private UIScreen InfiniteModalPrefab;

    private IEnumerator Start()
    {
      // give unity a sec to catch up so we can see animations effectively.
      yield return new WaitForSeconds(1.0f);

      // Show the first welcome screen using the ScreenManager.
      var welcomeScreen = _screenManager.ShowPrefab(WelcomeScreen1Prefab);
      // Wait for the screen to be dismissed. This waits until the screen completely
      // animates out.
      yield return welcomeScreen.WaitForDismiss();

      // Now that the animation has played out we'll show our main screen.
      yield return ShowWelcomeScreen2();


    }

    IEnumerator ShowWelcomeScreen2()
    {
      var welcomeScreen2 = _screenManager.ShowPrefab(WelcomeScreen2Prefab);
      while (true)
      {
        // Wait for the user to make a selection. This doesn't wait for the 
        // screen to animate out like WaitForDismiss().
        yield return welcomeScreen2.WaitForResult();

        // show a modal on top of our screen.
        yield return ShowSelectionModal();
        // WelcomeScreen2 does not despawn after being dismissed, so it will stick around forever.

      }
    }

    IEnumerator ShowSelectionModal()
    {
      // first we display our modal using UI Manager.
      var selectionScreen = _uiManager.ShowPrefab(SelectionPrefab);
      while (true)
      {
        // wait for a selection to be made.
        yield return selectionScreen.WaitForResult();
        if (selectionScreen.Result == 0)
        {
          // if zero is selected, we'll show a basic ok/cancel dialog 
          var subScreen = _uiManager.ShowConfirmCancel("Here is a title", "This is the message.");
          // wait for this result
          yield return subScreen.WaitForResult();
          // Show a second ok dialog displaying the value returned from the first. 
          var confirm =_uiManager.ShowConfirm(null, $"Your Choice was {subScreen.Result}");
          //since we want the result from the dialog, it sticks around until we manually despawn it.
          // this is configurable.
          subScreen.Despawn();
          yield return confirm.WaitForDismiss();
          confirm.Despawn();
        }

        if (selectionScreen.Result == 1)
        {
          // an example of what an infinite modal hierarchy would look like.
          yield return ShowInfiniteModal();
        }

        if (selectionScreen.Result == UIScreen.Cancel)
        {
          // We're done! close the modal hierarchy.
          yield break;
        }
      }
      
    }

    /// <summary>
    /// This is a recursive function that keeps displaying copies of the same modal.
    /// </summary>
    IEnumerator ShowInfiniteModal()
    {
      var modal = _uiManager.ShowPrefab(InfiniteModalPrefab);

      while (true)
      {
        yield return modal.WaitForResult();
        if (modal.Result == 0)
        {
          //pop another one onto the stack.
          yield return ShowInfiniteModal();
        }
        else
        {
          modal.DismissAndDespawn();
          yield break;
        }
      }
    }
  }
}