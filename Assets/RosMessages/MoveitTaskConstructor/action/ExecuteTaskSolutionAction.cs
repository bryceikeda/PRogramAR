using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.MoveitTaskConstructor
{
    public class ExecuteTaskSolutionAction : Action<ExecuteTaskSolutionActionGoal, ExecuteTaskSolutionActionResult, ExecuteTaskSolutionActionFeedback, ExecuteTaskSolutionGoal, ExecuteTaskSolutionResult, ExecuteTaskSolutionFeedback>
    {
        public const string k_RosMessageName = "msgs/ExecuteTaskSolutionAction";
        public override string RosMessageName => k_RosMessageName;


        public ExecuteTaskSolutionAction() : base()
        {
            this.action_goal = new ExecuteTaskSolutionActionGoal();
            this.action_result = new ExecuteTaskSolutionActionResult();
            this.action_feedback = new ExecuteTaskSolutionActionFeedback();
        }

        public static ExecuteTaskSolutionAction Deserialize(MessageDeserializer deserializer) => new ExecuteTaskSolutionAction(deserializer);

        ExecuteTaskSolutionAction(MessageDeserializer deserializer)
        {
            this.action_goal = ExecuteTaskSolutionActionGoal.Deserialize(deserializer);
            this.action_result = ExecuteTaskSolutionActionResult.Deserialize(deserializer);
            this.action_feedback = ExecuteTaskSolutionActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
