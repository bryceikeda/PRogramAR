 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class UpdateExpressionLists : MonoBehaviour
    {
        [SerializeField] ExpressionTree expressionTree;
        [SerializeField] ZoneObjectRuntimeDictionary zones;
        [SerializeField] ZonePropertyList zoneProperties;

        private void UpdateZones()
        {
            foreach (ExpressionTree.ExpressionNode node in expressionTree.expressionNodes)
            {
                if (node.expressionNodeName.Equals("Zones"))
                {
                    node.expressionNames.Clear();
                    node.nextExpressionNodes.Clear();

                    foreach (KeyValuePair<int, ZoneObject> pair in zones.Items)
                    {
                        node.expressionNames.Add("Zone " + pair.Key);
                        node.nextExpressionNodes.Add("Save");
                        node.expressionIconName.Add("Inbox");
                    }
                }
                if (node.expressionNodeName.Equals("ZonesSubGroup"))
                {
                    node.expressionNames.Clear();
                    node.nextExpressionNodes.Clear();

                    foreach (KeyValuePair<int, ZoneObject> pair in zones.Items)
                    {
                        node.expressionNames.Add("Zone " + pair.Key);
                        node.nextExpressionNodes.Add("Position");
                        node.expressionIconName.Add("Inbox");
                    }
                }
            }
        }

        private void OnEnable()
        {
            UpdateZones(); 
        }
    }
}
