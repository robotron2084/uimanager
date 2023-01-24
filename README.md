# UI Manager
A library for managing screens of UI for unity


Features

Display screens in a modal, with a shield.
Confirm Cancel Modal

TODO
Add ability for listeners to cancel out the closing of a modal.
Non-Modal Screens.
'Wizards' or Sequences of screens
UIManager as sub-manager, ie ensure that there is a clear path for re-use.

DONE
Some bugs with the screen stack to work out.
Don't hide modals when a new modal shows? Make that configurable?


## Random Mutterings

If I want to display a series of screens, with a back and forth system:

```csharp

var screenManager = new UIScreenManager();
var screen = screenManager.ShowScreen(screen1Prefab);
yield return WaitForResult();
if(result == UIScreen.Cancel)
{
    // go back.
    var prevScreen = screenManager.GoBack();
    yield return prevScreen.WaitForResult();
}else{
    var nextScreen = screenManager.ShowScreen(screen2Prefab);
    yield return nextScreen.WaitForResult();
}

```