using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vive/OriginCoordinates"), System.Serializable]

public class OriginCoordinates : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation; 
}
