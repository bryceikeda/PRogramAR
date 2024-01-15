 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProgramAR.Pages;
using System.IO;
/// <summary>
///
/// </summary>

namespace ProgramAR.Data
{
    public class SaveTest : MonoBehaviour
    {
        private string savePathExpression;
        private string savePathClause;
        private string savePathTAP;

        private Clause clause;
        private Expression expression; 

        public TriggerActionRules triggerActionRules;
        public ClauseRuntimeSet testSet; 

        private void Start()
        {
            savePathExpression = Application.persistentDataPath + "/expression.json";
            savePathClause = Application.persistentDataPath + "/clause.json";
            savePathTAP = Application.persistentDataPath + "/tap.json";

            expression = new Expression("Test1", "Test2", "Test3");
            clause = new Clause();
            clause.AddExpression(0, expression);
            clause.AddExpression(1, expression);
            testSet.Items.Add(clause); 
        }

        private void OnApplicationQuit()
        {
            using (StreamWriter stream = new StreamWriter(savePathTAP))
            {
                string save = JsonUtility.ToJson(testSet);
                stream.Write(save);
            }
         

        }
    }
}
