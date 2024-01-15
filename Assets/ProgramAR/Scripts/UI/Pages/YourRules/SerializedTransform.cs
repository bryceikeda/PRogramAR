using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    [System.Serializable]
    public class SerializedTransform
    {
        public float[] _position = new float[3];
        public float[] _rotation = new float[4];
        public float[] _scale = new float[3];
        public SerializedTransform(Transform transform, bool worldSpace = false)
        {
            _position[0] = transform.localPosition.x;
            _position[1] = transform.localPosition.y;
            _position[2] = transform.localPosition.z;

            _rotation[0] = transform.localRotation.x;
            _rotation[1] = transform.localRotation.y;
            _rotation[2] = transform.localRotation.z;
            _rotation[3] = transform.localRotation.w;

            _scale[0] = transform.localScale.x;
            _scale[1] = transform.localScale.y;
            _scale[2] = transform.localScale.z;
        }
    }
}
