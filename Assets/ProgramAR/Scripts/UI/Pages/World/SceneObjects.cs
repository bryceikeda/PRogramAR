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
    public class SceneObjects : MonoBehaviour, IUpdatePlanningSceneResponse
    {
        public const int numBoxes = 4;
        public const int numSpawn = 14; 
        ROSConnection m_Ros;
        public string topicName = "/unity/scene_objects";

        [SerializeField] BoxObjectRuntimeDictionary boxObjects;
        [SerializeField] BoxObjectFactory boxFactory;
        [SerializeField] List<GameObject> boxes;
        [SerializeField] List<GameObject> surfaces;
        [SerializeField] Transform world;
        [SerializeField] Transform spawnPos;
        [SerializeField] List<CheckPointInsideBox> checkPoint; 
        [SerializeField] UpdatePlanningSceneEvent updatePlanningSceneEvent;
        [SerializeField] List<GameObject> boxPadding; 
        private SceneObjectsMsg sceneObjects;
        bool objectsAdded = false;

        // Publish the cube's position and rotation every N seconds
        public float publishMessageFrequency = 0.5f;

        private void Start()
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.RegisterPublisher<SceneObjectsMsg>(topicName);
            //SpawnObject("Box 1", new Vector3(-0.1460001f, .7171f, .657f), new Quaternion(0f, 0f, 0f, 1f));

            foreach (GameObject ob in boxes)
            {
                boxObjects.Add(ob.name, ob.GetComponent<BoxObject>());
            }
            
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
            StartCoroutine(UpdatePlanningSceneObjects(movingObject));
        }

        public void SpawnObject(string boxName, Vector3 position, Quaternion rotation)
        {
            BoxObject box = boxFactory.GetInstance();
            box.transform.parent = transform;
            box.transform.localPosition = position;
            box.transform.localRotation = rotation; 
            box.boxName = boxName;
            box.gameObject.name = boxName;
            box.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            boxObjects.Add(boxName, box);
            boxes.Add(box.gameObject);
        }

        public void AddPlanningSceneObjects()
        {
            sceneObjects = new SceneObjectsMsg
                (
                    new SceneObjectMsg[numSpawn]
                    {
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg(),
                        new SceneObjectMsg()
                    }
                );
            int paddingIndex = 0; 
            for (int i = 0; i < numSpawn; i++)
            {
                if (i < numBoxes)
                {
                    if (boxes[i].CompareTag("Box"))
                    {
                        Debug.Log(surfaces[i].tag + " " + i);
                        sceneObjects.objects[i].type = SceneObjectMsg.BOX;
                    }
                    else if (boxes[i].CompareTag("Object"))
                    {
                        sceneObjects.objects[i].type = SceneObjectMsg.CYLINDER;
                    }

                    sceneObjects.objects[i].operation = SceneObjectMsg.ADD;
                    sceneObjects.objects[i].id = boxes[i].name;
                    var dim = boxes[i].transform.localScale.To<FLU>();
                    sceneObjects.objects[i].dimensions = new Vector3Msg(Math.Abs(dim.x), Math.Abs(dim.y), Math.Abs(dim.z));
                    var angles = world.InverseTransformDirection(boxes[i].transform.rotation.eulerAngles);
                    var newRot = Quaternion.Euler(0f, angles.y, 0f);
                    var boxPos = world.InverseTransformPoint(boxes[i].transform.position);

                    for (int k = 0; k < checkPoint.Count; k++)
                    {
                        if (checkPoint[k].IsInside(boxes[i].transform.position))
                        {
                            var newPos = new Vector3(boxPos.x, surfaces[k].transform.localPosition.y + surfaces[k].transform.localScale.y / 2 + boxes[i].transform.localScale.y / 2, boxPos.z);
                            sceneObjects.objects[i].spawn_pose = new PoseMsg(newPos.To<FLU>(), newRot.To<FLU>());
                        }
                    }
                }
                else if(i < 10)
                {
                    if (surfaces[(i - numBoxes)].CompareTag("Box"))
                    {
                        sceneObjects.objects[i].type = SceneObjectMsg.BOX;
                        sceneObjects.objects[i].operation = SceneObjectMsg.ADD;
                        sceneObjects.objects[i].id = surfaces[(i - numBoxes)].name;
                        var dim = surfaces[(i - numBoxes)].transform.localScale.To<FLU>();
                        sceneObjects.objects[i].dimensions = new Vector3Msg(Math.Abs(dim.x), Math.Abs(dim.y), Math.Abs(dim.z));
                        sceneObjects.objects[i].spawn_pose = new PoseMsg(surfaces[(i - numBoxes)].transform.localPosition.To<FLU>(), surfaces[(i - numBoxes)].transform.localRotation.To<FLU>());
                    }
                    else if (surfaces[(i - numBoxes)].CompareTag("Cylinder"))
                    {
                        sceneObjects.objects[i].type = SceneObjectMsg.CYLINDER;
                        sceneObjects.objects[i].operation = SceneObjectMsg.ADD;
                        sceneObjects.objects[i].id = surfaces[(i - numBoxes)].name;
                        var dim = surfaces[(i - numBoxes)].transform.localScale.To<FLU>();
                        sceneObjects.objects[i].dimensions = new Vector3Msg(Math.Abs(dim.x), Math.Abs(dim.y), Math.Abs(dim.z)*2);
                        sceneObjects.objects[i].spawn_pose = new PoseMsg(surfaces[(i - numBoxes)].transform.localPosition.To<FLU>(), surfaces[(i - numBoxes)].transform.localRotation.To<FLU>());
                    }
                }
                else
                {
                    sceneObjects.objects[i].type = SceneObjectMsg.BOX;
                    sceneObjects.objects[i].operation = SceneObjectMsg.ADD;
                    sceneObjects.objects[i].id = boxPadding[paddingIndex].name;
                    var dim = boxPadding[paddingIndex].transform.localScale.To<FLU>();
                    sceneObjects.objects[i].dimensions = new Vector3Msg(Math.Abs(dim.x), Math.Abs(dim.y), Math.Abs(dim.z));
                    sceneObjects.objects[i].spawn_pose = new PoseMsg(boxPadding[paddingIndex].transform.localPosition.To<FLU>(), Quaternion.identity.To<FLU>());
                    paddingIndex++; 
                }
            }

            m_Ros.Publish(topicName, sceneObjects);
            Debug.Log("[SceneObjects]: Initializing planning scene.");
            objectsAdded = true;
        }

        IEnumerator UpdatePlanningSceneObjects(string movingObject)
        {
            yield return new WaitForSeconds(2);
            if (objectsAdded)
            {
                sceneObjects = new SceneObjectsMsg
                (
                    new SceneObjectMsg[8]
                    {
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg(),
                       new SceneObjectMsg()
                    }
                );

                int paddingIndex = 0;
                for (int i = 0; i < numBoxes; i++)
                {

                    sceneObjects.objects[i].operation = SceneObjectMsg.MOVE;

                    sceneObjects.objects[i].id = boxes[i].name;
                    if (movingObject.Equals(boxes[i].name))
                    {
                        var angles = world.InverseTransformDirection(boxes[i].transform.rotation.eulerAngles);
                        var newRot = Quaternion.Euler(0f, angles.y, 0f);
                        var boxPos = world.InverseTransformPoint(boxes[i].transform.position);

                        for (int k = 0; k < checkPoint.Count; k++)
                        {
                            if (checkPoint[k].IsInside(boxes[i].transform.position))
                            {
                                var newPos = new Vector3(boxPos.x, surfaces[k].transform.localPosition.y + surfaces[k].transform.localScale.y / 2 + boxes[i].transform.localScale.y / 2, boxPos.z);
                                sceneObjects.objects[i].spawn_pose = new PoseMsg(newPos.To<FLU>(), newRot.To<FLU>());
                            }
                        }

                        sceneObjects.objects[7].operation = SceneObjectMsg.MOVE;
                        sceneObjects.objects[7].id = boxPadding[3].name;
                        sceneObjects.objects[7].spawn_pose = new PoseMsg(new PointMsg(sceneObjects.objects[i].spawn_pose.position.x, sceneObjects.objects[i].spawn_pose.position.y, sceneObjects.objects[i].spawn_pose.position.z + boxes[i].transform.localScale.y / 2 + .1f), Quaternion.identity.To<FLU>());
                    }
                    else
                    {
                        var angles = world.InverseTransformDirection(boxes[i].transform.rotation.eulerAngles);
                        var newRot = Quaternion.Euler(0f, angles.y, 0f);
                        var boxPos = world.InverseTransformPoint(boxes[i].transform.position);

                        for (int k = 0; k < checkPoint.Count; k++)
                        {
                            if (checkPoint[k].IsInside(boxes[i].transform.position))
                            {
                                var newPos = new Vector3(boxPos.x, 0.1f + surfaces[k].transform.localPosition.y + surfaces[k].transform.localScale.y / 2 + boxes[i].transform.localScale.y / 2, boxPos.z);
                                sceneObjects.objects[i].spawn_pose = new PoseMsg(newPos.To<FLU>(), newRot.To<FLU>());
                            }
                        }
                        sceneObjects.objects[4 + paddingIndex].operation = SceneObjectMsg.MOVE;
                        sceneObjects.objects[4 + paddingIndex].id = boxPadding[paddingIndex].name;
                        sceneObjects.objects[4 + paddingIndex].spawn_pose = new PoseMsg(new PointMsg(sceneObjects.objects[i].spawn_pose.position.x, sceneObjects.objects[i].spawn_pose.position.y, sceneObjects.objects[i].spawn_pose.position.z), Quaternion.identity.To<FLU>());
                        paddingIndex++;
                    }



                    Debug.Log("[SceneObjects]: Updated Scene");

                    m_Ros.Publish(topicName, sceneObjects);
                }
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

