using UnityEngine;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.ObjectRecognition; 
using RosMessageTypes.Moveit;
using RosMessageTypes.Std; 
using System.Collections.Generic;
using RosMessageTypes.Shape;
using RosMessageTypes.SceneHandler;
using System.Collections;
using System;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class SceneObjectsCopyCustom : MonoBehaviour, IUpdatePlanningSceneResponse
    {
        public const int numBoxes = 1; 
        ROSConnection m_Ros;
        public string topicName = "/unity/scene_objects";

        [SerializeField] BoxObjectRuntimeDictionary boxObjects;
        [SerializeField] BoxObjectFactory boxFactory;
        [SerializeField] List<GameObject> boxes;
        [SerializeField] Transform world;
        [SerializeField] Transform spawnPos;
        [SerializeField] UpdatePlanningSceneEvent updatePlanningSceneEvent; 
        private PlanningSceneWorldMsg scene;

        [SerializeField] List<GameObject> worldCollisionObjects;

        private SceneObjectsMsg sceneObjects; 

        bool objectsAdded = false;

        // Publish the cube's position and rotation every N seconds
        public float publishMessageFrequency = 0.5f;

        private void Start()
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.RegisterPublisher<SceneObjectsMsg>(topicName); 
            SpawnObject("Box 1", new Vector3(-0.09200002f, .7171f, 1.023901f), new Quaternion(0f, 0f, 0f, 1f));
            StartCoroutine(AddObjectsToPlanningScene());
        }

        IEnumerator AddObjectsToPlanningScene()
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(3);
            AddPlanningSceneObjects();
        }

        public void OnUpdatePlanningSceneEvent(string movingObject)
        {
            UpdatePlanningSceneObjects(movingObject);
        }

        public void SpawnObject(string boxName, Vector3 position, Quaternion rotation)
        {
            BoxObject box = boxFactory.GetInstance();
            box.transform.parent = transform;
            box.transform.localPosition = position;
            box.transform.localRotation = rotation; 
            box.boxName = boxName;
            box.gameObject.name = boxName; 
            boxObjects.Add(boxName, box);
            boxes.Add(box.gameObject);
        }

        public void AddPlanningSceneObjects()
        {
            sceneObjects = new SceneObjectsMsg
                (
                    new SceneObjectMsg[numBoxes]
                    {
                        new SceneObjectMsg()

                    }
                );

            for (int i = 0; i < numBoxes; i++)
            {
                sceneObjects.objects[i].type = SceneObjectMsg.BOX;
                sceneObjects.objects[i].operation = SceneObjectMsg.ADD;
                sceneObjects.objects[i].id = boxes[i].name;
                var dim = boxes[i].transform.localScale.To<FLU>();
                sceneObjects.objects[i].dimensions = new Vector3Msg(Math.Abs(dim.x), Math.Abs(dim.y), Math.Abs(dim.z));
                sceneObjects.objects[i].spawn_pose = new PoseMsg(boxes[i].transform.localPosition.To<FLU>(), boxes[i].transform.localRotation.To<FLU>());
                Debug.Log(boxes[i].transform.localPosition.x + ", " + boxes[i].transform.localPosition.y + ", " + boxes[i].transform.localPosition.z);
                Debug.Log(sceneObjects.objects[i].spawn_pose.position.x + ", " + sceneObjects.objects[i].spawn_pose.position.y + ", " + sceneObjects.objects[i].spawn_pose.position.z );
            }

            m_Ros.Publish(topicName, sceneObjects);
            Debug.Log("[SceneObjects]: Initializing planning scene.");
            objectsAdded = true;
        }

        public void UpdatePlanningSceneObjects(string movingObject)
        {
            if (objectsAdded)
            {
                for (int i = 0; i < scene.collision_objects.Length; i++)
                {
                    scene.collision_objects[i].pose.position = boxes[i].transform.localPosition.To<FLU>();
                    scene.collision_objects[i].pose.orientation = boxes[i].transform.localRotation.To<FLU>();
                    scene.collision_objects[i].operation = CollisionObjectMsg.MOVE;
                    
                }
                m_Ros.Publish(topicName, scene);
            }
        }

        void OnEnable()
        {
            updatePlanningSceneEvent.RegisterListener(this);
        }
        
        void OnDisable()
        {
            updatePlanningSceneEvent.UnregisterListener(this);
        }
    }
}

