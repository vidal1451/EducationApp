using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject panelFade;
    [SerializeField] Transform panelParent;
    [Header("# Panel Finished")]
    [SerializeField] GameObject panelFinishedPrefab;
    [HideInInspector] public GameObject panelFinishedTemp;
    [HideInInspector] public GameObject panelOptionsTemp;

    [Header("# Panel Options")]
    [SerializeField] GameObject panelOptionsPref;
    public static UIController ins;
    private void Awake() {
        ins = this;
    }
    private void Update() {
         if (Input.GetKeyDown(KeyCode.Escape) && panelOptionsTemp==null && panelFinishedTemp == null)
        {
            OnClickOptions();
        }
    }
    public void ClosePanelOptions(){
        DestroyPanel();
        Destroy(panelOptionsTemp);
    }
    public void OnClickOptions(){
        ShowPanelFade();
        panelOptionsTemp = Instantiate(panelOptionsPref,panelParent);
        panelOptionsTemp.GetComponent<PanelOptions>().SetEvents(ClosePanelOptions,delegate{LoadScene("Menu");});
    }
    public void GameFinished(UnityAction _action){
        Debug.Log("gamefinished");
        panelFinishedTemp = Instantiate(panelFinishedPrefab,panelParent);
        panelFinishedTemp.GetComponent<InfoPanel>().SetContinueBtn(_action);
        panelFinishedTemp.SetActive(true);
    }
    public void ShowPanelFade(){
        //GameObject panelTemp = Instantiate(panelFade,panelParent);
        StartCoroutine(FadeImage(true));
    }
    public void DestroyPanel(){
        //GameObject panelTemp = GameObject.FindGameObjectWithTag("PanelFade");
        StartCoroutine(FadeImage(false));
    }
    IEnumerator FadeImage(bool fadeAway)
    {
        Color color = panelFade.GetComponent<Image>().color;

        // fade from opaque to transparent
        if (!fadeAway)
        {
            // loop over 1 second backwards
            color.a =0f;
            panelFade.GetComponent<Image>().color = color;
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                color.a = i;
                panelFade.GetComponent<Image>().color = color;
                yield return null;
            }
            panelFade.SetActive(false);
        }
        // fade from transparent to opaque
        else
        {
            Debug.Log("fade off");
            // loop over 1 second
            color.a =0.85f;
            panelFade.GetComponent<Image>().color = color;
            panelFade.SetActive(true);

            for (float i = 0; i <= 0.85; i += Time.deltaTime)
            {
                // set color with i as alpha
                color.a = i;
                panelFade.GetComponent<Image>().color = color;
                yield return null;
            }
            
        }
    }
    public void LoadScene(string _scene){
        SceneManager.LoadScene(_scene);
    }
}
