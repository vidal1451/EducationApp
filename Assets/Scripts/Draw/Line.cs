using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Draw
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [Header("# Colors")]
        [SerializeField] Color[] startColor;
        [SerializeField] Color[] endColor;

        public void SetColor(int _startColor, int _endColor){
            lineRenderer.startColor = startColor[_startColor];
            lineRenderer.endColor = endColor[_endColor];
        }
        public void SetWith(float _withLine){
            lineRenderer.startWidth = _withLine;
        }

        public void SetPosition(Vector2 pos){
            if (!CanAppend(pos)) return;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount-1,pos);
        }
        private bool CanAppend(Vector2 pos){
            if (lineRenderer.positionCount ==0) return true;
            return Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount-1),pos) > DrawManager.RESOLUTION;
        }
    }

}
