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


    private IEnumerator Start()
    {
      // give unity a sec to catch up so we can see animations effectively.
      yield return new WaitForSeconds(1.0f);
      
      //create an instance of the welcome screen.
      var welcomeScreen = _uiManager.ShowPrefab(WelcomePrefab);
      // wait for it to close
      yield return welcomeScreen.WaitForClose();
      // we're done with it, let's go ahead and clean it up.
      welcomeScreen.Despawn();
      
      var selectionScreen = _uiManager.ShowPrefab(SelectionPrefab);
      while (true)
      {
        yield return selectionScreen.WaitForResult();
        //based upon the result, lets's show another ui.
        var subScreen = ScreenForResult(selectionScreen.Result);
        yield return subScreen.WaitForResult();
        var confirm =_uiManager.ShowConfirm(null, $"Your Choice was {subScreen.Result}");
        subScreen.Despawn();
        yield return confirm.WaitForClose();
      }

    }

    private UIScreen ScreenForResult(int selectionScreenResult)
    {
      switch (selectionScreenResult)
      {
        case UIScreen.Confirm:
          return _uiManager.ShowConfirmCancel("This is the title.", "This is the message.");
      }

      return null;
    }
  }
}