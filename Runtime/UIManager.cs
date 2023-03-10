using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public class UIManager : MonoBehaviour, IUIController
  {
    [SerializeField]
    private Transform _root;

    public Transform Root
    {
      get => _root;
    }

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
    
    private List<IUIScreen> screens = new List<IUIScreen>();
    private List<IUIScreen> screensToShow = new List<IUIScreen>();
    private List<IUIScreen> screensToHide = new List<IUIScreen>();
    public ClickShield ShieldPrefab;
    private IEnumerator _shieldTransition;
    private bool _dirty = false;

    void Awake()
    {
      Shield = Instantiate(ShieldPrefab);
    }

    void Update()
    {
      if (_dirty)
      {
        _dirty = false;
        foreach (var uiScreen in screensToShow)
        {
          if (screens.Contains(uiScreen))
          {
            throw new Exception("Screen already shown!");
          }
          screens.Add(uiScreen);
          var pos = Vector3.zero;
          pos.z = _step * screens.Count;
          uiScreen.Transform.localPosition = pos;
          StartCoroutine(uiScreen.TransitionIn());

        }
        foreach (var uiScreen in screensToHide)
        {
          screens.Remove(uiScreen);
          StartCoroutine(uiScreen.TransitionOut());
        }

        UpdateShield();

        screensToShow.Clear();
        screensToHide.Clear();
      }
    }

    private void MarkDirty()
    {
      _dirty = true;
    }
    
    public UIScreen ShowConfirm(string title, string message)
    {
      var retVal = ShowPrefab(ConfirmCancelDialogPrefab);
      retVal.InitConfirm(title, message);
      return retVal;
    }

    public UIScreen ShowConfirmCancel(string title, string message)
    {
      var retVal = ShowPrefab(ConfirmCancelDialogPrefab);
      retVal.InitConfirmCancel(title, message);
      return retVal;
    }

    public void ShowScreen(IUIScreen screen)
    {
      screen.Controller = this;
      screen.Transform.SetParent(_root, false);
      screensToShow.Add(screen);
      MarkDirty();
    }

    public T ShowPrefab<T>(T screenPrefab) where T : UIScreen
    {
      var screen = Instantiate(screenPrefab);
      ShowScreen(screen);
      return screen;
    }

    private void UpdateShield()
    {
      if (screens.Count > 0)
      {
        // reposition the shield.
        var farthestForwardScreen = screens[screens.Count - 1];
        var pos = farthestForwardScreen.Transform.localPosition;
        pos.z -= _shieldDepth;
        _shield.Transform.localPosition = pos;
        _shield.Transform.SetAsLastSibling();
        int siblingIndex = farthestForwardScreen.Transform.GetSiblingIndex();
        _shield.Transform.SetSiblingIndex(siblingIndex);
        
      }

      if (_shieldTransition != null)
      {
        StopCoroutine(_shieldTransition);
        _shieldTransition = null;
      }

      _shieldTransition = UpdateShieldRoutine();
      StartCoroutine(_shieldTransition);
    }

    private IEnumerator UpdateShieldRoutine()
    {
      // wait a frame because we want to see where we are after multiple updates.
      yield return null;
      if (screens.Count == 0)
      {
        yield return _shield.TransitionOut();
      }
      else
      {
        yield return _shield.TransitionIn();
      }

      _shieldTransition = null;
    }

    public void HideScreen(IUIScreen screen)
    {
      screensToHide.Add(screen);
      MarkDirty();
    }

    public void HideScreen(UIScreen uiScreen)
    {
      throw new NotImplementedException();
    }

    public IEnumerator WaitUntilActive(GameObject active)
    {
      yield return new WaitUntil(() => active.activeSelf);
    }

  }
}