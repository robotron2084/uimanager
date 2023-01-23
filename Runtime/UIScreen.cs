using System.Collections;
using GameJamStarterKit.FXSystem;
using UnityEngine;

namespace com.enemyhideout.ui
{
  [RequireComponent(typeof(Signaller))]
  [RequireComponent(typeof(Animator))]
  public class UIScreen : MonoBehaviour, IUIScreen
  {
    
    private Animator _animator;
    private Signaller _signaller;
    private int _result;

    public int Result
    {
      get => _result;
      set => _result = value;
    }
    
    private const string Dismissed = "Dismissed";
    private const string ResultSelected = "ResultSelected";

    public const int Confirm = 0;
    public const int Cancel = -1;
    
    void Awake()
    {
      _animator = GetComponent<Animator>();
      _signaller = GetComponent<Signaller>();
      gameObject.SetActive(false);
    }
    
    public Transform Transform
    {
      get
      {
        return transform;
      }
    }

    public UIManager Manager { get; set; }

    public void Dismiss()
    {
      Manager.HideScreen(this);
    }

    public IEnumerator TransitionIn()
    {
      gameObject.SetActive(true);
      _animator.SetBool("Visible", true);
      yield return _signaller.WaitForSignal(Signaller.Complete);
    }

    public IEnumerator TransitionOut()
    {
      _animator.SetBool("Visible", false);
      yield return _signaller.WaitForSignal(Signaller.Complete);
      _signaller.RaiseSignal(Dismissed);
    }

    public IEnumerator WaitForClose()
    {
      yield return _signaller.WaitForSignal(Dismissed);
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
      yield return Manager.WaitUntilActive(gameObject);
      yield return _signaller.WaitForSignal(ResultSelected);
    }
  }
}