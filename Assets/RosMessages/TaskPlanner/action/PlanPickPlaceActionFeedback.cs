using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.TaskPlanner
{
    public class PlanPickPlaceActionFeedback : ActionFeedback<PlanPickPlaceFeedback>
    {
        public const string k_RosMessageName = "task_planner/PlanPickPlaceActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public PlanPickPlaceActionFeedback() : base()
        {
            this.feedback = new PlanPickPlaceFeedback();
        }

        public PlanPickPlaceActionFeedback(HeaderMsg header, GoalStatusMsg status, PlanPickPlaceFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static PlanPickPlaceActionFeedback Deserialize(MessageDeserializer deserializer) => new PlanPickPlaceActionFeedback(deserializer);

        PlanPickPlaceActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = PlanPickPlaceFeedback.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.feedback);
        }


#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
