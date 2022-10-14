using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PanelOptions : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Button exitAppButton;

    public void SetEvents(UnityAction _closeBtn, UnityAction _exitBtn){
        closeButton.onClick.AddListener(_closeBtn);
        exitAppButton.onClick.AddListener(_exitBtn);
    }
}
