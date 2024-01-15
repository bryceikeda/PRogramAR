//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.TaskPlanner
{
    [Serializable]
    public class PlanPickPlaceResult : Message
    {
        public const string k_RosMessageName = "task_planner/PlanPickPlace";
        public override string RosMessageName => k_RosMessageName;

        public bool success;
        public MoveitTaskConstructor.SolutionMsg solution;

        public PlanPickPlaceResult()
        {
            this.success = false;
            this.solution = new MoveitTaskConstructor.SolutionMsg();
        }

        public PlanPickPlaceResult(bool success, MoveitTaskConstructor.SolutionMsg solution)
        {
            this.success = success;
            this.solution = solution;
        }

        public static PlanPickPlaceResult Deserialize(MessageDeserializer deserializer) => new PlanPickPlaceResult(deserializer);

        private PlanPickPlaceResult(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            this.solution = MoveitTaskConstructor.SolutionMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.solution);
        }

        public override string ToString()
        {
            return "PlanPickPlaceResult: " +
            "\nsuccess: " + success.ToString() +
            "\nsolution: " + solution.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Result);
        }
    }
}