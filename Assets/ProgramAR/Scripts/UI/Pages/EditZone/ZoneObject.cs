using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;
using ProgramAR.Variables;
using System.Collections.Generic;
using TMPro;

namespace ProgramAR.Pages
{
    public class ZoneObject : MonoBehaviour
    {
        [SerializeField] FloatRangeVariable enableAlpha;
        [SerializeField] FloatRangeVariable disableAlpha;
        [SerializeField] List<string> collidingBoxes = new List<string>();
        [SerializeField] TextMeshPro title;
        
        private bool isInteractive;
        private string zoneName;
        public ZoneObject(Color color)
        {
            GetComponent<Renderer>().material.color = color;
            EnableZoneInteraction();
        }

        public string GetZoneName()
        {
            return zoneName; 
        }

        public void SetZoneName(string zoneName)
        {
            this.zoneName = zoneName;
            title.text = zoneName;
        }
        public Vector3 GetPlacePosition()
        {
            return transform.position; 
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Object"))
            {
                var boxName = collision.gameObject.GetComponent<BoxObject>().boxName;
                if (!collidingBoxes.Contains(boxName))
                {
                    collidingBoxes.Add(boxName);
                }
            }
        }

        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Object"))
            {
                var boxName = collision.gameObject.GetComponent<BoxObject>().boxName;
                if (collidingBoxes.Contains(boxName))
                {
                    collidingBoxes.Remove(boxName);
                }
            }
        }

        public List<string> GetObjectsInside()
        {
            return collidingBoxes;
        }

        public bool HasObjectsInside()
        {
            return collidingBoxes.Count > 0; 
        }
        public bool HasObjectInside(string boxName)
        {
            return collidingBoxes.Contains(boxName);
        }

        public void ToggleInteraction()
        {
            if (!isInteractive)
            {
                DisableZoneInteraction();
            }
            else
            {
                EnableZoneInteraction(); 
            }
        }

        public void EnableZoneInteraction()
        {
            isInteractive = false;
            var alpha = GetComponent<Renderer>().material.color;
            alpha.a = enableAlpha.Value;
            GetComponent<Renderer>().material.color = alpha;
            SetZoneInteraction(true);
        }

        public void DisableZoneInteraction()
        {
            isInteractive = true;
            var alpha = GetComponent<Renderer>().material.color;
            alpha.a = disableAlpha.Value;
            GetComponent<Renderer>().material.color = alpha;
            SetZoneInteraction(false); 
        }

        private void SetZoneInteraction(bool enable)
        {
            GetComponent<ObjectManipulator>().enabled = enable;
            GetComponent<NearInteractionGrabbable>().enabled = enable;
            GetComponent<BoundsControl>().enabled = enable;
        }

        public void SetZoneColor(Color color)
        {
            GetComponent<Renderer>().material.color = color;
        }
    }
}