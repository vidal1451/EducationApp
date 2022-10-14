using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Draw
{
public class DrawAlphabet : MonoBehaviour
{
    private Camera cam;
    public const float RESOLUTION = 0.1f;
    private Line currentLine;
    [Header("# Line Prefab")]
    [SerializeField] private Line linePrefab;
    [SerializeField] Transform lineParent;
    
    [Header("# Draw Exercises")]
    [SerializeField] List<GameObject> linesPrefab = new List<GameObject>();

    [Header("# Panel References")]
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject characterMessage;
    [SerializeField] GameObject character;
    [SerializeField ] List<int> pointList = new List<int>();
    GameObject objectToPaint;
    LineToDraw currentObjLeft;

    [Header("# Line Properties")]
    [Range(0,1)] [SerializeField] float lineWidth =0.4f;

    [Header("# Variables")]
    [SerializeField] int target;
    [SerializeField] float distance;
    [SerializeField] float distance2;
    [Range(0,1)]
    [SerializeField] float distanceBetwenTarget=0.3f;
    [Range(0,1)] [SerializeField] float errorRange = 0.5f;
    Vector2 mousePos, targetPos, targetPos2;
    float distancePanel;
    int lineIndex;
    bool firstStep, secondStep;
    public bool isTouched;
    bool completed;
    int colorIndex;
    void Start()
    {
        cam = Camera.main;
        firstStep=true;
        ShowCurrentLine();
    }
    void Update()
    {
        if (isTouched==true){
            if (firstStep==true){
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                targetPos = currentObjLeft.targetList[target].transform.position;
                if(target>0) targetPos2 = currentObjLeft.targetList[target-1].transform.position;
                distance = Vector2.Distance(targetPos,mousePos);
                distance2 = Vector2.Distance(targetPos2,mousePos);
                //Vector2 sd= new Vector2(lineParent.position.x,lineParent.position.y);
                if(Input.GetMouseButtonDown(0) && firstStep==true){
                    currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);  
                    currentLine.SetColor(colorIndex,colorIndex);   
                    currentLine.SetWith(lineWidth);
                } 
                if (Input.GetMouseButton(0) && firstStep==true){
                    if (currentLine!=null) currentLine.SetPosition(mousePos);
                    if (distance <=distanceBetwenTarget){
                        pointList.Add(target);
                        if(target<currentObjLeft.targetCount-1){
                            target++;
                        } 
                        else {
                            firstStep=false;
                            Debug.Log("Congrats! You have completed the line");
                            target=0;
                            NextStep();
                        }
                    }
                    if(distance>=distanceBetwenTarget+errorRange && distance2 >=distanceBetwenTarget+errorRange){
                        firstStep=false;
                        AudioManager.instance.PlayErrorSound();
                        ResetDraw();
                    }
                }
            }
            if (completed==true)
            {
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                distancePanel = Vector2.Distance(objectToPaint.transform.GetChild(0).transform.position,mousePos);
                if(Input.GetMouseButtonDown(0) && distancePanel<=2.5){
                    sendButton.SetActive(true);
                    currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);  
                    currentLine.SetWith(lineWidth);
                    currentLine.SetColor(colorIndex,colorIndex);   
                } 
                if (Input.GetMouseButton(0) && distancePanel <=2.5){
                    if (currentLine!=null) currentLine.SetPosition(mousePos);
                } 
            }  
        }
    }
    public void OnChangeDraw(int _index){
        Destroy(currentObjLeft.transform.parent.gameObject); 
        switch (_index)
        {
            case 0:
                    if (lineIndex == 0) lineIndex = linesPrefab.Count-1;
                    else lineIndex--;
                    break;

            case 1:
                    if (lineIndex ==linesPrefab.Count-1) lineIndex =0;
                    else lineIndex++;
                    break;
        }
        ShowCurrentLine();
    }
    public void SetTouch(bool _touch){
        isTouched=_touch;
    }
    public void SetColor(int _color){
        colorIndex = _color;
    }
    public void ShowCurrentLine(){
        GameObject lineTemp =  Instantiate(linesPrefab[lineIndex].gameObject, lineParent);
        currentObjLeft= lineTemp.transform.GetChild(0).GetComponent<LineToDraw>();
        objectToPaint = lineTemp.transform.GetChild(1).gameObject;
        //objectToPaint.GetComponent<Image>().sprite = currentObjLeft.objetiveSprite;
        lineTemp.SetActive(true);
        firstStep=true;
        ResetDraw();
    }
    void NextStep(){
        character.SetActive(true);
        Animator chartr = character.GetComponent<Animator>();
        chartr.SetBool("excited",true);
        characterMessage.SetActive(true);
        StartCoroutine(ShowMessage());
    }
    IEnumerator ShowMessage(){
        yield return new WaitForSeconds(4f);
        character.SetActive(false);
        characterMessage.SetActive(false);
        completed=true;
        objectToPaint.SetActive(true);
    }

    public void FinishGame(){
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }
        UIController.ins.ShowPanelFade();
        objectToPaint.SetActive(false);
        UIController.ins.GameFinished(delegate{ContinueBtn();});
    }
    public void ContinueBtn(){
        Debug.Log("Continue");
        UIController.ins.DestroyPanel();
        Destroy(UIController.ins.panelFinishedTemp); 
        OnChangeDraw(1);
        currentLine=null;
        pointList.Clear();
        target=0;
    }
    public void ResetDraw(){
        Debug.Log("reseting");
        currentLine=null;
        pointList.Clear();
        completed=false;
        target=0;
        StartCoroutine(DestroyLines());
    }
    IEnumerator DestroyLines(){
        yield return new WaitForSeconds(1f);
        firstStep=true;
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }

    }
}
    
}

