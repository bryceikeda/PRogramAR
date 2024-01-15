using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.TaskPlanner
{
    public class PlanPickPlaceActionResult : ActionResult<PlanPickPlaceResult>
    {
        public const string k_RosMessageName = "task_planner/PlanPickPlaceActionResult";
        public override string RosMessageName => k_RosMessageName;


        public PlanPickPlaceActionResult() : base()
        {
            this.result = new PlanPickPlaceResult();
        }

        public PlanPickPlaceActionResult(HeaderMsg header, GoalStatusMsg status, PlanPickPlaceResult result) : base(header, status)
        {
            this.result = result;
        }
        public static PlanPickPlaceActionResult Deserialize(MessageDeserializer deserializer) => new PlanPickPlaceActionResult(deserializer);

        PlanPickPlaceActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = PlanPickPlaceResult.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.result);
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
