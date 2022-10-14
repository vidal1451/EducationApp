using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Draw
{
    public class DrawManagerTwo : MonoBehaviour
{
    private Camera cam;
    public const float RESOLUTION = 0.1f;
    private Line currentLine;
    [Header("# Line Prefab")]
    [SerializeField] float distancePanel;
    [SerializeField] private Line linePrefab;
    [SerializeField] Transform lineParent;
    [Header("# Colors")]
    [SerializeField] Image[] currentColor;
    [SerializeField] Color[] colorList;
    
    [Header("# Draw")]
    [SerializeField] List<LineToDraw> linesPrefab = new List<LineToDraw>();
    [SerializeField] List<int> pointList = new List<int>();
    [SerializeField] List<int> tempList= new List<int>();
    [SerializeField] LineToDraw currentObj;
    [SerializeField] GameObject animal;

    [Header("# Panel References")]
    [SerializeField] GameObject[] animals;
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject textInfo;

    [Header("# Variables")]
    [SerializeField] int target;
    [SerializeField] List<float> distances = new List<float>();
    [SerializeField] List<Vector2> targetList = new List<Vector2>();
    [SerializeField] float distanceBetwenTarget;
    Vector2 mousePos, targetPos;
    int lineIndex;
    bool trace, paint;
    public bool isTouched;
    bool completed;
    int colorIndex;
    float withLine;
    void Start()
    {
        cam = Camera.main;
        trace=true;
        ShowCurrentLine();
        SetColor(0);
        SetWith(0.35f);
    }
    void AssignTargets(){
        int index = 0;
        foreach (RectTransform item in linesPrefab[lineIndex].transform)
        {
            targetList.Add(item.position);
            index++;
        }
    }
    void CalculateDistance(){
        distances.Clear();
        for (int i = 0; i < targetList.Count; i++)
        {
            distances.Add(Vector2.Distance(targetList[i],mousePos));
        }
    }
    bool CheckDistance(){
        for (int i = 0; i < distances.Count; i++)
        {
            if (distances[i] <=distanceBetwenTarget){target=i; return true;} 
        }
        return false;
    }
    void Update()
    {
        
        if (isTouched==true){
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            CalculateDistance();
            if(Input.GetMouseButtonDown(0) && trace==true){
                currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);  
                currentLine.SetWith(withLine);
                currentLine.SetColor(colorIndex,colorIndex);   
            } 
            if (Input.GetMouseButton(0) && trace==true){
                
                if (CheckDistance()){
                    if (currentLine!=null) currentLine.SetPosition(mousePos);
                    if (!tempList.Contains(target)) tempList.Add(target);
                    if ((pointList.Count+tempList.Count) >= currentObj.targetCount-1){
                        trace=false;
                        paint=true;
                        Debug.Log("Congrats! You have completed the line");
                        textInfo.SetActive(true);
                        completed=true;
                        target=0;
                    }
                }
                if(!CheckDistance()){
                    trace=false; 
                    AudioManager.instance.PlayErrorSound();
                    DeleteLastLine();      
                }
            }
            
        }
        if(Input.GetMouseButtonUp(0) && trace == true) { 
                Debug.Log("STOP");
                foreach (int target in tempList) { if(!pointList.Contains(target)) {pointList.Add(target);}}
                tempList.Clear();
            }
        if (completed==true)
            {
                if (isTouched==true){
                    
                    mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    distancePanel = Vector2.Distance(lineParent.position,mousePos);
                    if(Input.GetMouseButtonDown(0) && paint==true && distancePanel<=5.3f){
                        textInfo.SetActive(false);
                        sendButton.SetActive(true);
                        currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);  
                        currentLine.SetWith(withLine);
                        currentLine.SetColor(colorIndex,colorIndex);   
                    } 
                    if (Input.GetMouseButton(0) && paint==true && distancePanel <=5.3f){
                        if (currentLine!=null) currentLine.SetPosition(mousePos);
                    } 
                }
            }
    }
    public void OnChangeDraw(int _index){
        Destroy(currentObj.gameObject); 
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
        foreach (Image image in currentColor)
        {
            image.color = colorList[colorIndex];
        }
    }
    public void SetWith(float _withLine){
        withLine=_withLine;
    }
    public void ShowCurrentLine(){
        GameObject lineTemp =  Instantiate(linesPrefab[lineIndex].gameObject, lineParent);
        currentObj= lineTemp.GetComponent<LineToDraw>();
        lineTemp.SetActive(true);
        ResetDraw();
        AssignTargets();
    }
    public void FinishGame(){
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }
        UIController.ins.ShowPanelFade();
        foreach (GameObject animal in animals)
        {
            animal.SetActive(false);
        }
        UIController.ins.GameFinished(ContinueBtn);
    }
    void ContinueBtn(){
        UIController.ins.DestroyPanel();
        Destroy(UIController.ins.panelFinishedTemp); 
        OnChangeDraw(1);
    }
    public void ResetDraw(){
        Debug.Log("reseting");
        currentLine=null;
        pointList.Clear();
        target=0;
        tempList.Clear();
        completed=false;
        targetList.Clear();
        textInfo.SetActive(false);
        sendButton.SetActive(false);
        StartCoroutine(DeleteLines());
    }
    void DeleteLastLine(){
        tempList.Clear();
        StartCoroutine(DeleteLine());
    }
    IEnumerator DeleteLine(){
        yield return new WaitForSeconds(2f);
        trace=true;
        Destroy(currentLine.gameObject);
    }
    IEnumerator DeleteLines(){
        yield return new WaitForSeconds(1f);
        trace=true;
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }

    }

}

    
}
