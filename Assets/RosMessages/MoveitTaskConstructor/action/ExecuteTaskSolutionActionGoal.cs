using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.MoveitTaskConstructor
{
    public class ExecuteTaskSolutionActionGoal : ActionGoal<ExecuteTaskSolutionGoal>
    {
        public const string k_RosMessageName = "moveit_task_constructor_msgs/ExecuteTaskSolutionActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public ExecuteTaskSolutionActionGoal() : base()
        {
            this.goal = new ExecuteTaskSolutionGoal();
        }

        public ExecuteTaskSolutionActionGoal(HeaderMsg header, GoalIDMsg goal_id, ExecuteTaskSolutionGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static ExecuteTaskSolutionActionGoal Deserialize(MessageDeserializer deserializer) => new ExecuteTaskSolutionActionGoal(deserializer);

        ExecuteTaskSolutionActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = ExecuteTaskSolutionGoal.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.goal_id);
            serializer.Write(this.goal);
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
