using UnityEngine;

namespace ProgramAR.Pages
{ 
    public class FixedScale : MonoBehaviour
    {

        public float fixedScale = 1;
        public GameObject parent;

        // Update is called once per frame
        void Update()
        {
            transform.localScale = new Vector3(fixedScale / parent.transform.localScale.x, fixedScale / parent.transform.localScale.y, fixedScale / parent.transform.localScale.z);
        }
    }
}
