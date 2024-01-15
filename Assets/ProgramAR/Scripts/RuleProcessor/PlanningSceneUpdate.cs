using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosMessageTypes.Moveit; 

namespace RuleProcessor
{
    public class PlanningSceneUpdate : MonoBehaviour
    {
        ROSConnection m_Ros;
        public string topicName = "/unity/update_planning_scene";

        void Start()
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.RegisterPublisher<PlanningSceneMsg>(topicName);
        }

    }
}
