using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;
using ProgramAR.Variables;
using System.Collections;

public class ViveTrackerReciever : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    private Double[] float_array;
    private int port = 8054;

    [SerializeField] OriginCoordinates offset;

    [SerializeField] List<GameObject> trackerObjects;

    [SerializeField] MatrixTransform trans;
    [SerializeField] BoolVariable calibrated;

    public Transform worldOrigin;

    public OriginCoordinates orig;
    public OriginCoordinates rotationOffset;
    Vector3 vec;
    Quaternion quat;

    public Transform qrOrigin;

    public bool appliedOffset = false;

    // Use this for initialization
    void Start()
    {
        float_array = new double[7 * trackerObjects.Count];
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        vec = new Vector3();
        quat = new Quaternion();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < trackerObjects.Count; i++)
        {
            vec.x = (float)float_array[7 * i];
            vec.y = (float)float_array[7 * i + 1];
            vec.z = (float)float_array[7 * i + 2];
            quat.x = (float)float_array[7 * i + 3];
            quat.y = (float)float_array[7 * i + 4];
            quat.z = (float)float_array[7 * i + 5];
            quat.w = (float)float_array[7 * i + 6];

            vec = ConvertViveToUnityVector(vec);
            quat = ConvertViveToUnityQuaternion(quat);
            if (calibrated.Value)
            {

                var newCoord = trans.trans * Matrix4x4.TRS(vec, GetNormalized(quat), trackerObjects[i].transform.localScale);
                trackerObjects[i].transform.localPosition = ExtractTranslationFromMatrix(ref newCoord);
                trackerObjects[i].transform.localRotation = ExtractRotationFromMatrix(ref newCoord);
                /*                if (!appliedOffset)
                                {
                                    var offset = trackerObjects[1].transform.localPosition + qrOrigin.InverseTransformPoint(worldOrigin.position);
                                    var Mpos = new Matrix4x4();
                                    Mpos.SetColumn(0, new Vector4(1, 0, 0, 0));
                                    Mpos.SetColumn(1, new Vector4(0, 1, 0, 0));
                                    Mpos.SetColumn(2, new Vector4(0, 0, 1, 0));
                                    Mpos.SetColumn(3, new Vector4(-offset.x, -offset.y, -offset.z, 1));
                                    trans.trans *= Mpos; 
                                    appliedOffset = true; 
                                }*/

            }
            else
            {
                trackerObjects[i].transform.localPosition = vec;
                trackerObjects[i].transform.localRotation = quat;
            }
        }

    }

    public static Quaternion GetNormalized(Quaternion q)
    {
        float f = 1f / Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        return new Quaternion(q.x * f, q.y * f, q.z * f, q.w * f);
    }
    private Vector3 ConvertViveToUnityVector(Vector3 vec)
    {
        return new Vector3(-vec.x, vec.y, vec.z);
    }

    private Quaternion ConvertViveToUnityQuaternion(Quaternion quat)
    {
        var rot = new Quaternion(-quat.z, -quat.y, quat.x, quat.w);
        Quaternion rot180 = Quaternion.AngleAxis(180.0f, Vector3.up);
        Quaternion rot90 = Quaternion.AngleAxis(90.0f, Vector3.forward);

        return rot * rot90 * rot180;
    }

    public void ResetOrigin()
    {
        calibrated.Value = false; 
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();
        client.Close();
    }
    //https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=8446435
    public void SetOrigin()
    {
        trans.trans = new Matrix4x4();
        orig.position = new Vector3();
        calibrated.Value = false;
        Vector3 Pz = Vector3.zero;
        Vector3 Po = Vector3.zero;
        Vector3 Px = Vector3.zero;

        int divisor = 0;

        // To calibrate, Set the trackers on each QR block
        // Average the position based on the QR origin, objects currently parented to world
        while (divisor != 1000)
        {
            var pz = trackerObjects[0].transform.localPosition;
            var po = trackerObjects[1].transform.localPosition;
            var px = trackerObjects[2].transform.localPosition;
            Pz += pz;
            Po += po;
            Px += px;
            divisor++;
        }
        Pz /= divisor;
        Po /= divisor;
        Px /= divisor;

        var xHat = Px - Po;
        var zHat = Pz - Po;

        xHat.Normalize();
        zHat.Normalize();

        var yHat = Vector3.Cross(zHat, xHat);

        trans.trans.SetColumn(0, new Vector4(xHat.x, yHat.x, zHat.x, 0));
        trans.trans.SetColumn(1, new Vector4(xHat.y, yHat.y, zHat.y, 0));
        trans.trans.SetColumn(2, new Vector4(xHat.z, yHat.z, zHat.z, 0));
        trans.trans.SetColumn(3, new Vector4(0, 0, 0, 1));

        orig.position = Po;

        var Mpos = new Matrix4x4();
        Mpos.SetColumn(0, new Vector4(1, 0, 0, 0));
        Mpos.SetColumn(1, new Vector4(0, 1, 0, 0));
        Mpos.SetColumn(2, new Vector4(0, 0, 1, 0));
        Mpos.SetColumn(3, new Vector4(-orig.position.x, -orig.position.y, -orig.position.z, 1));

        trans.trans *= Mpos;
        Debug.Log("Calibrated");
        calibrated.Value = true;
    }


    public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 translate;
        translate.x = matrix.m03;
        translate.y = matrix.m13;
        translate.z = matrix.m23;
        return translate;
    }

    /// <summary>
    /// Extract rotation quaternion from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Quaternion representation of rotation transform.
    /// </returns>
    public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }




    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        print("Starting Server");
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
                byte[] data = client.Receive(ref anyIP);
                for (int i = 0; i < data.Length; i++)
                {
                    float_array[i] = BitConverter.ToDouble(data, i * 8);
                }
            }
            catch (Exception err)
            {
                bool debug = false;
                if (debug)
                {
                    print(err.ToString());
                }
            }
        }
    }

}