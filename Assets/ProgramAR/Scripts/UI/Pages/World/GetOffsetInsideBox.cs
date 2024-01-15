using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class GetOffsetInsideBox : MonoBehaviour
    {
        public Transform pointForBB;
        public float forwardOffset;
        public float sidewaysOffset;
        public float xPos;
        public float yPos;
        void OnDrawGizmos()
        {
            bool pointInsideBB = InsideBox(pointForBB.position, transform.position, transform.localScale * 0.5f, transform.rotation);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pointForBB.position, .1f);
            Gizmos.color = pointInsideBB ? Color.cyan : Color.red;
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.matrix *= cubeTransform;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = oldGizmosMatrix;
        }

        public bool IsInside(Vector3 position)
        {
            return InsideBox(position, transform.position, transform.localScale * 0.5f, transform.rotation); 
        }

        public static bool InsideBox(Vector3 p, Vector3 center, Vector3 extends, Quaternion rotation)
        {
            Matrix4x4 m = Matrix4x4.TRS(center, rotation, Vector3.one);
            p = m.inverse.MultiplyPoint3x4(p);
            //p = rotation * p - center;
            return p.x <= extends.x && p.x > -extends.x
                && p.y <= extends.y && p.y > -extends.y
                && p.z <= extends.z && p.z > -extends.z;
        }
    }
}
