using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectsInFrontOfFace : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;

    [SerializeField] float offset = .1f; 
    private void Start()
    {
        Transform trans = Camera.main.transform;
        var pos = new Vector3(trans.position.x, trans.position.y, trans.position.z+ offset);
        foreach (GameObject obj in objects)
        {
            obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
        }
    }
}
