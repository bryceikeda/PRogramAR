using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.MoveitTaskConstructor
{
    public class ExecuteTaskSolutionActionResult : ActionResult<ExecuteTaskSolutionResult>
    {
        public const string k_RosMessageName = "moveit_task_constructor_msgs/ExecuteTaskSolutionActionResult";
        public override string RosMessageName => k_RosMessageName;


        public ExecuteTaskSolutionActionResult() : base()
        {
            this.result = new ExecuteTaskSolutionResult();
        }

        public ExecuteTaskSolutionActionResult(HeaderMsg header, GoalStatusMsg status, ExecuteTaskSolutionResult result) : base(header, status)
        {
            this.result = result;
        }
        public static ExecuteTaskSolutionActionResult Deserialize(MessageDeserializer deserializer) => new ExecuteTaskSolutionActionResult(deserializer);

        ExecuteTaskSolutionActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = ExecuteTaskSolutionResult.Deserialize(deserializer);
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
