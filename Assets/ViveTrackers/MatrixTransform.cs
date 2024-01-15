using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vive/MatrixTransform"), System.Serializable]

public class MatrixTransform : ScriptableObject
{
    public Matrix4x4 trans; 
}
