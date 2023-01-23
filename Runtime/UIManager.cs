using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public class UIManager : MonoBehaviour
  {
    [SerializeField]
    private Transform _root;

    [SerializeField] private ConfirmCancelDialog ConfirmCancelDialogPrefab;

      /// <summary>
    /// Amount of space between each screen.
    /// </summary>
    private float _step = 100.0f;

    private float _shieldDepth = -50.0f; //this is relative to the screen's z 
    private ClickShield _shield;
    private ClickShield Shield
    {
      get
      {
        return _shield;
      }
      set
      {
        _shield = value;
        _shield.transform.SetParent(_root, false);
        _shield.gameObject.SetActive(false);
      }
    }

    public ClickShield ShieldPrefab;
    
    

    void Awake()
    {
      Shield = Instantiate(ShieldPrefab);
    }
    
    private List<IUIScreen> screens = new List<IUIScreen>();
    
    public UIScreen ShowConfirm(string title, string message)
    {
      var retVal = ShowPrefab(ConfirmCancelDialogPrefab);
      retVal.InitConfirm(title, message);
      ShowScreen(retVal);
      return retVal;
    }


    public UIScreen ShowConfirmCancel(string title, string message)
    {
      var retVal = ShowPrefab(ConfirmCancelDialogPrefab);
      retVal.InitConfirmCancel(title, message);
      ShowScreen(retVal);
      return retVal;
    }

    
    public void ShowScreen(IUIScreen screen)
    {
      StartCoroutine(ShowScreenCoroutine(screen));
    }

    public T ShowPrefab<T>(T screenPrefab) where T : UIScreen
    {
      var screen = Instantiate(screenPrefab);
      StartCoroutine(ShowScreenCoroutine(screen));
      return screen;
    }

    public IEnumerator ShowScreenCoroutine(IUIScreen screen)
    {
      screen.Manager = this;
      screen.Transform.SetParent(_root, false);
      if (screens.Count == 0)
      {
        yield return _shield.TransitionIn();
      }
      screens.Add(screen);
      var pos = Vector3.zero;
      pos.z = _step * screens.Count;
      screen.Transform.localPosition = pos;
      pos.z -= _shieldDepth;
      _shield.Transform.localPosition = pos;
      yield return screen.TransitionIn();
    }
    
    public void HideScreen(IUIScreen screen)
    {
      StartCoroutine(HideScreenCoroutine(screen));
    }

    public IEnumerator WaitUntilActive(GameObject active)
    {
      yield return new WaitUntil(() => active.activeSelf);
    }
    

    public IEnumerator HideScreenCoroutine(IUIScreen screen)
    {
      screens.Remove(screen);
      yield return screen.TransitionOut();
      if (screens.Count == 0)
      {
        yield return _shield.TransitionOut();
      }
      else
      {
        var oldScreen = screens[screens.Count - 1];
        yield return oldScreen.TransitionIn();
      }
    }

  }
}