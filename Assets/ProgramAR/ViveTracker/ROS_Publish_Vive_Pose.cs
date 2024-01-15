using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector; 
using RosMessageTypes.Geometry; 

public class ROS_Publish_Vive_Pose : MonoBehaviour
{

    //private float timeElapsed; 
    //spublic float publishFrequency = 0.1f;


    private ROSConnection ros; 
    public GameObject ViveGlobal;
    public GameObject ViveMesh; 
    public GameObject ViveOrigin; 
    public bool firstTSet = false;

    public Vector3 firstPos; 
    public Quaternion firstRot; 


    
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance(); 
        ros.RegisterPublisher<PoseMsg>("vive_pose"); 
    }

    // Update is called once per frame
    void Update()
    {

        if(!firstTSet){
            //Save Frame 0 origin
            ViveOrigin.transform.position = ViveGlobal.transform.position; 
            ViveOrigin.transform.rotation = ViveGlobal.transform.rotation;
            firstPos = ViveGlobal.transform.position; 
            firstRot = ViveGlobal.transform.rotation; 

            //Move Mesh to origin
            ViveMesh.transform.position = ViveOrigin.transform.position; 
            ViveMesh.transform.rotation = ViveOrigin.transform.rotation; 

            //parent mesh to origin
            ViveMesh.transform.SetParent(ViveOrigin.transform);  
            firstTSet = true; 
            return; 
        }

        if(firstTSet){
            
            //Update mesh with global tracking
            ViveMesh.transform.position = ViveGlobal.transform.position;
            ViveMesh.transform.rotation = ViveGlobal.transform.rotation; 

            // timeElapsed += Time.deltaTime;
            // //if (!(timeElapsed > publishFrequency)) return;

            var pose = new PoseMsg(
                new PointMsg(ViveMesh.transform.localPosition.x, ViveMesh.transform.localPosition.y, ViveMesh.transform.localPosition.z),
                new QuaternionMsg(ViveMesh.transform.localRotation.w, ViveMesh.transform.localRotation.x, ViveMesh.transform.localRotation.y, ViveMesh.transform.localRotation.z)
            ); 
            ros.Publish("vive_pose", pose);
            Debug.Log(ViveMesh.transform.localPosition);
            Debug.Log(ViveMesh.transform.localRotation);  
            //timeElapsed = 0;
        }
    }
}
