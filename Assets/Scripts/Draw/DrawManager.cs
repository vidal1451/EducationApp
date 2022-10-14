using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Draw
{
    public class DrawManager : MonoBehaviour
{
    private Camera cam;
    public const float RESOLUTION = 0.1f;
    private Line currentLine;
    [Header("# Line Prefab")]
    [SerializeField] private Line linePrefab;
    [SerializeField] Transform lineParent;
    
    [Header("# Draw")]
    [SerializeField] List<GameObject> linesPrefab = new List<GameObject>();
    [SerializeField] List<int> pointList = new List<int>();
    [SerializeField] LineToDraw currentObj;
    [SerializeField] GameObject animal;

    [Header("# Panel References")]
    [SerializeField] GameObject objective;
    [SerializeField] GameObject character;

    [Header("# Variables")]
    [SerializeField] int target;
    [SerializeField] float distance;
    [SerializeField] float distanceBetwenTarget;
    Vector2 mousePos, targetPos;
    int lineIndex;
    bool istrue;
    public bool isTouched;
    bool completed;
    int colorIndex;
    void Start()
    {
        cam = Camera.main;
        istrue=true;
        ShowCurrentLine();
    }
    void Update()
    {
        
        if (isTouched==true){
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            targetPos = currentObj.targetList[target].transform.position;
            distance = Vector2.Distance(targetPos,mousePos);
            //Vector2 sd= new Vector2(lineParent.position.x,lineParent.position.y);
            if(Input.GetMouseButtonDown(0) && istrue==true){
                currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);  
                currentLine.SetColor(colorIndex,colorIndex);   
            } 
            if (Input.GetMouseButton(0) && istrue==true){
                if (currentLine!=null) currentLine.SetPosition(mousePos);
                if (distance <=distanceBetwenTarget){
                    pointList.Add(target);
                    if(target<currentObj.targetCount-1){
                        target++;
                    } 
                    else {
                        istrue=false;
                        Debug.Log("Congrats! You have completed the line");
                        character.GetComponent<Animator>().SetBool("walking",true);
                        completed=true;
                        target=0;
                    }
                }
                if(distance>=distanceBetwenTarget+0.8f){
                    istrue=false;
                    AudioManager.instance.PlayErrorSound();
                    ResetDraw();
                }
            } 
        }
        if (completed==true)
            {
                character.transform.position = Vector2.MoveTowards(character.transform.position, currentObj.targetList[target].transform.position, 4f * Time.deltaTime);
                if (Vector2.Distance(character.transform.position,currentObj.targetList[target].transform.position)<=0.1f){
                    if (target<currentObj.targetCount-1) target++;
                    else {
                        completed=false;
                        FinishGame();
                    }
                }
            }
    }
    public void OnChangeDraw(int _index){
        Destroy(currentObj.transform.parent.gameObject); 
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
        currentObj= lineTemp.transform.GetChild(0).GetComponent<LineToDraw>();
        character = lineTemp.transform.GetChild(1).gameObject;
        objective.GetComponent<Image>().sprite = currentObj.objetiveSprite;
        objective.SetActive(true);
        istrue=true;
        ResetDraw();
    }
    public void FinishGame(){
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }
        UIController.ins.ShowPanelFade();
        character.SetActive(false);
        objective.SetActive(false);
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
        istrue=true;
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Lines");
        foreach (var item in lines)
        {
            Destroy(item);
        }

    }

}

    
}
