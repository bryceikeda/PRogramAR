using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.MoveitTaskConstructor
{
    public class ExecuteTaskSolutionActionFeedback : ActionFeedback<ExecuteTaskSolutionFeedback>
    {
        public const string k_RosMessageName = "moveit_task_constructor_msgs/ExecuteTaskSolutionActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public ExecuteTaskSolutionActionFeedback() : base()
        {
            this.feedback = new ExecuteTaskSolutionFeedback();
        }

        public ExecuteTaskSolutionActionFeedback(HeaderMsg header, GoalStatusMsg status, ExecuteTaskSolutionFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static ExecuteTaskSolutionActionFeedback Deserialize(MessageDeserializer deserializer) => new ExecuteTaskSolutionActionFeedback(deserializer);

        ExecuteTaskSolutionActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = ExecuteTaskSolutionFeedback.Deserialize(deserializer);
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
