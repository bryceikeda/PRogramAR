using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Geometry; 
using RosMessageTypes.BuiltinInterfaces; 
using Valve.VR; 
public class ViveManager : MonoBehaviour
{

    [Tooltip("Root Vive Tracker")]
    public GameObject Vive1Local;
    [Tooltip("Object Reference for getting Vive1 pose and rotation")]
    public GameObject Vive1Global; 

    [Tooltip("Secondary Vive Tracker to orient vive coordinate space")]
    public GameObject Vive2Local;

    [Tooltip("Object Reference for getting Vive2 pose and rotation")]
    public GameObject Vive2Global;  

    public GameObject Vive3Local; 

    public GameObject Vive3Global; 
    private ROSConnection ros; //Ros Connection
    private float seconds = 0.0f; //Time Stamp for pose array message

    public string ros_topic; //ROS Topic Name

    void Start()
    {
        //Setup Ros connection
        //ros = ROSConnection.GetOrCreateInstance(); 
        //ros.RegisterPublisher<PoseArrayMsg>(ros_topic); 
    }

    // Update is called once per frame
    void Update()
    {   
        
        //Update Vive Tracker Locals with Global Transforms
        Vive1Local.transform.position = Vive1Global.transform.position; 
        Vive2Local.transform.position = Vive2Global.transform.position; 
        Vive3Local.transform.position = Vive3Global.transform.position; 

        Vive1Local.transform.rotation = Vive1Global.transform.rotation; 
        Vive2Local.transform.rotation = Vive2Global.transform.rotation; 
        Vive3Local.transform.rotation = Vive3Global.transform.rotation; 

        //Setup Vive1 message
        var Vive1Pose = new PoseMsg(
            new PointMsg(Vive1Local.transform.localPosition.x, Vive1Local.transform.localPosition.y, Vive1Local.transform.localPosition.z),
            new QuaternionMsg(Vive1Global.transform.rotation.x, Vive1Global.transform.rotation.y, Vive1Global.transform.rotation.z, Vive1Global.transform.rotation.w)
        ); 
 
        //Setup Vive2 message
        var Vive2Pose = new PoseMsg(
            new PointMsg(Vive2Local.transform.localPosition.x, Vive2Local.transform.localPosition.y, Vive2Local.transform.localPosition.z),
            new QuaternionMsg(Vive2Local.transform.localRotation.x, Vive2Local.transform.localRotation.y, Vive2Local.transform.localRotation.z, Vive2Local.transform.localRotation.w)
        ); 

        var Vive3Pose = new PoseMsg(
            new PointMsg(Vive3Local.transform.localPosition.x, Vive3Local.transform.localPosition.y, Vive3Local.transform.localPosition.z),
            new QuaternionMsg(Vive3Local.transform.localRotation.x, Vive3Local.transform.localRotation.y, Vive3Local.transform.localRotation.z, Vive3Local.transform.localRotation.w)
        ); 

        PoseMsg[] poseMessage = new PoseMsg[3]{Vive1Pose, Vive2Pose, Vive3Pose};
        var poseArray = new PoseArrayMsg();

        //Assign poses
        poseArray.poses = poseMessage; 

        //Time stamp  
        seconds += Time.deltaTime; 
        var t = new TimeMsg((uint) seconds, (uint) seconds * 1000); 
        poseArray.header.stamp = t; 
        
        //Header Message
        poseArray.header.frame_id = Time.frameCount.ToString();
        
        //Publish Pose
        //ros.Publish("vive_pose", poseArray);        
    }
}
