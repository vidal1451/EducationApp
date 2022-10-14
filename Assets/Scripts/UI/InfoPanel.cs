using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class InfoPanel : MonoBehaviour
{
    [SerializeField] Animator panelAnimator;
    [Header("# Starts")]
    [SerializeField] Transform starsPanel;
    [SerializeField] GameObject[] starPref;
    [Header("# Buttons")]
    [SerializeField] Button continueBtn;
    private void Start() {
        StartCoroutine(InstantiateStars());
    }
    public void ClosePanel(){
        panelAnimator.SetBool("Finish",true);
    }
    IEnumerator InstantiateStars(){
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject star in starPref)
        {
            star.SetActive(true);
        }
    }
    public void SetContinueBtn(UnityAction _continueAction){
        continueBtn.onClick.AddListener(_continueAction);

    }
}
