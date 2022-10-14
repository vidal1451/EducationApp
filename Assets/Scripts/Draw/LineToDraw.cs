using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Draw
{
    public class LineToDraw : MonoBehaviour
    {
        [Header("# Line Sprite")]
        public Image lineSprite;
        public Sprite objetiveSprite;

        [Header("# Line Properties")]
        [SerializeField] Transform targetContainer;
        public List<Transform> targetList = new List<Transform>();
        public int targetCount;
        private void Start() {
            foreach (Transform tgt in targetContainer)
            {
                targetList.Add(tgt);
            }
            targetCount = targetList.Count;
        }
    }
}

