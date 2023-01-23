using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace com.enemyhideout.ui
{
  public class ConfirmCancelDialog : UIScreen
  {
    [SerializeField] private Transform _buttonsContainer;
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private TMP_Text _messageLabel;
    [SerializeField] private ButtonWithLabel _buttonPrefab;
    
    public class ButtonData
    {
      public string Label;
      public int Result;
    }

    public static ButtonData ConfirmButtonData = new ButtonData
    {
      Label = "Ok",
      Result = 0
    };
    
    public static ButtonData CancelButtonData = new ButtonData
    {
      Label = "Cancel",
      Result = -1
    };

    public static List<ButtonData> ConfirmButtons = new List<ButtonData>()
    {
      ConfirmButtonData
    };

    public static List<ButtonData> ConfirmCancelButtons = new List<ButtonData>()
    {
      ConfirmButtonData, CancelButtonData
    };


    public void InitConfirm(string title, string message)
    {
      Init(title, message, ConfirmButtons);
    }

    public void InitConfirmCancel(string title, string message)
    {
      Init(title, message, ConfirmCancelButtons);
    }

    public void Init(string title, string message, IEnumerable<ButtonData> buttons)
    {
      gameObject.name = title + "-"+string.Join("-", buttons.Select(x => x.Label));
      InitLabel(_titleLabel, title);
      InitLabel(_messageLabel, message);
      BuildButtons(_buttonsContainer, SetResultAndDismiss, _buttonPrefab, buttons);
    }

    private static void InitLabel(TMP_Text label, string text)
    {
      bool hasValue = !string.IsNullOrEmpty(text);
      label.gameObject.SetActive(hasValue);
      label.text = text;
    }

    private static void BuildButtons(Transform root,
      Action<int> setResultCallback,
      ButtonWithLabel buttonPrefab,
      IEnumerable<ButtonData> buttons)
    {
      foreach (var buttonData in buttons)
      {
        ButtonWithLabel button = Instantiate(buttonPrefab);
        button.transform.SetParent(root, false);
        button.Label.text = buttonData.Label;
        button.Button.onClick.AddListener(() => setResultCallback(buttonData.Result));
      }
    }
  }
}