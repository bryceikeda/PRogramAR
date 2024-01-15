using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.TaskPlanner
{
    public class PlanPickPlaceActionGoal : ActionGoal<PlanPickPlaceGoal>
    {
        public const string k_RosMessageName = "task_planner/PlanPickPlaceActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public PlanPickPlaceActionGoal() : base()
        {
            this.goal = new PlanPickPlaceGoal();
        }

        public PlanPickPlaceActionGoal(HeaderMsg header, GoalIDMsg goal_id, PlanPickPlaceGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static PlanPickPlaceActionGoal Deserialize(MessageDeserializer deserializer) => new PlanPickPlaceActionGoal(deserializer);

        PlanPickPlaceActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = PlanPickPlaceGoal.Deserialize(deserializer);
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
