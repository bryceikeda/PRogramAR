using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProgramAR.Pages
{
    public class TextHeadFollower : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
