using System.Collections;
using GameJamStarterKit.FXSystem;
using UnityEngine;

namespace com.enemyhideout.ui
{
  /// <summary>
  /// The base class for screens, both modal and non modal. There are a few requirements here:
  ///   * A <see cref="Signaller"/> that can be used to await for signals from the Animator and also raise signals.
  ///   * An Animator configured with transition in/out that sends a Complete signal once the animations are complete.
  ///     For more info in this area, check out the examples.
  /// 
  /// Screens have the following properties and phases:
  ///   * Transition In - Modals being in an inactive state by default. The modal transitions in, becomes active and visible. 
  ///   * Transition Out - The modal transitions out, and becomes inactive.
  ///   * Result - When a user has finished interacting with the screen, a Result is set to a value.
  ///     By default a value of zero is Success. Use the WaitForResult() coroutine to wait for this value to be set.
  ///   * Dismissed - A screen is 'dismissed' when it is removed from its controller. It will no longer be displayed in the future.
  ///   * Despawn - After a result has been returned, or the modal has been dismissed, its time to destroy the screen by
  ///     calling Despawn. By default this calls Destroy();
  /// 
  /// </summary>
  [RequireComponent(typeof(Signaller))]
  [RequireComponent(typeof(Animator))]
  public class UIScreen : MonoBehaviour, IUIScreen
  {
    
    private Animator _animator;
    private Signaller _signaller;
    private int _result = int.MinValue;
    private bool _despawnOnDismiss;

    /// <summary>
    /// If this is set, calling Dismiss will also Despawn the screen.
    /// </summary>
    public bool DespawnOnDismiss
    {
      get => _despawnOnDismiss;
      set => _despawnOnDismiss = value;
    }

    /// <summary>
    /// The result that defines a return value. This should be set via SetResult
    /// in order for listeners to WaitForResult() to react to the change.
    /// </summary>
    public int Result
    {
      get => _result;
      set => _result = value;
    }
    
    /// <summary>
    /// When a screen is dismissed, this signal will be raised. Used by WaitForDismiss()
    /// </summary>
    private const string Dismissed = "Dismissed";
    
    /// <summary>
    /// When a result is selected, this signal will be raised. Used by WaitForResult()
    /// </summary>
    private const string ResultSelected = "ResultSelected";

    /// <summary>
    /// Defines a successful result for a screen.
    /// </summary>
    public const int Confirm = 0;
    
    /// <summary>
    /// Defines a Cancel or unsuccessful result for a screen.
    /// </summary>
    public const int Cancel = -1;
    
    void Awake()
    {
      _animator = GetComponent<Animator>();
      _signaller = GetComponent<Signaller>();
      gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Used by the controller to reparent and position the screen in the hierarchy.
    /// </summary>
    public Transform Transform
    {
      get
      {
        return transform;
      }
    }

    /// <summary>
    /// The current controller. This is set by the controller when a screen is added.
    /// </summary>
    public IUIController Controller { get; set; }

    /// <summary>
    /// Transitions out and removes the screen from its controller.
    /// </summary>
    public void Dismiss()
    {
      Controller.HideScreen(this);
    }

    /// <summary>
    /// Called by the controller to display this screen. Not called by consumers.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionIn()
    {
      gameObject.SetActive(true);
      _animator.SetBool("Visible", true);
      yield return _signaller.WaitForSignal(Signaller.Complete);
    }

    /// <summary>
    /// Called by the controller to hide this screen. Not called by consumers.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionOut()
    {
      _animator.SetBool("Visible", false);
      yield return _signaller.WaitForSignal(Signaller.Complete);
      _signaller.RaiseSignal(Dismissed);
      if (_despawnOnDismiss)
      {
        Despawn();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listener"></param>
    /// <returns></returns>
    public IEnumerator WaitForDismiss(object listener=null)
    {
      yield return _signaller.WaitForSignal(Dismissed, listener);
    }

    public void Despawn()
    {
      Destroy(gameObject);
    }

    public void SetResult(int val)
    {
      _result = val;
      _signaller.RaiseSignal(ResultSelected);
    }

    public void SetResultAndDismiss(int val)
    {
      SetResult(val);
      Dismiss();
    }

    public IEnumerator WaitForResult()
    {
      yield return Controller.WaitUntilActive(gameObject);
      yield return _signaller.WaitForSignal(ResultSelected);
    }

    public void DismissAndDespawn()
    {
      _despawnOnDismiss = true;
      Dismiss();
    }

    public void SetResultAndDespawn(int result)
    {
      SetResult(result);
      DismissAndDespawn();
    }
  }
}