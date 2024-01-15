 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class InitializeExpressionButtons : MonoBehaviour
    {
        public List<GameObject> expressions;
        public GameObject expressionButton;
        private bool initialized = false; 

        public void Awake()
        {
            if (initialized == false)
            {
                foreach (GameObject obj in expressions)
                {
                    obj.SetActive(false);
                }
                expressionButton.SetActive(false);
                initialized = true; 
            }
        }
    }
}
