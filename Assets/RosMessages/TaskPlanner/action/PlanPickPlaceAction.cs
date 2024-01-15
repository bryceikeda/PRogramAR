using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.TaskPlanner
{
    public class PlanPickPlaceAction : Action<PlanPickPlaceActionGoal, PlanPickPlaceActionResult, PlanPickPlaceActionFeedback, PlanPickPlaceGoal, PlanPickPlaceResult, PlanPickPlaceFeedback>
    {
        public const string k_RosMessageName = "task_planner/PlanPickPlaceAction";
        public override string RosMessageName => k_RosMessageName;


        public PlanPickPlaceAction() : base()
        {
            this.action_goal = new PlanPickPlaceActionGoal();
            this.action_result = new PlanPickPlaceActionResult();
            this.action_feedback = new PlanPickPlaceActionFeedback();
        }

        public static PlanPickPlaceAction Deserialize(MessageDeserializer deserializer) => new PlanPickPlaceAction(deserializer);

        PlanPickPlaceAction(MessageDeserializer deserializer)
        {
            this.action_goal = PlanPickPlaceActionGoal.Deserialize(deserializer);
            this.action_result = PlanPickPlaceActionResult.Deserialize(deserializer);
            this.action_feedback = PlanPickPlaceActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
