//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.MoveitTaskConstructor
{
    [Serializable]
    public class TaskDescriptionMsg : Message
    {
        public const string k_RosMessageName = "moveit_task_constructor_msgs/TaskDescription";
        public override string RosMessageName => k_RosMessageName;

        //  unique id of this task
        public string task_id;
        //  list of all stages, including the task stage itself
        public StageDescriptionMsg[] stages;

        public TaskDescriptionMsg()
        {
            this.task_id = "";
            this.stages = new StageDescriptionMsg[0];
        }

        public TaskDescriptionMsg(string task_id, StageDescriptionMsg[] stages)
        {
            this.task_id = task_id;
            this.stages = stages;
        }

        public static TaskDescriptionMsg Deserialize(MessageDeserializer deserializer) => new TaskDescriptionMsg(deserializer);

        private TaskDescriptionMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.task_id);
            deserializer.Read(out this.stages, StageDescriptionMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.task_id);
            serializer.WriteLength(this.stages);
            serializer.Write(this.stages);
        }

        public override string ToString()
        {
            return "TaskDescriptionMsg: " +
            "\ntask_id: " + task_id.ToString() +
            "\nstages: " + System.String.Join(", ", stages.ToList());
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
