using System.Collections.Generic;
using UnityEngine;
using ProgramAR.Pages;
/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class BoxObject : MonoBehaviour
    {
        [SerializeField] List<string> collidingZones = new List<string>();
        public string boxName;

        public bool IsInsideZone(string zoneName)
        {
            return collidingZones.Contains(zoneName);
        }



        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Zone"))
            {
                var zoneName = collision.gameObject.GetComponent<ZoneObject>().GetZoneName();
                if (!collidingZones.Contains(zoneName))
                {
                    collidingZones.Add(zoneName);
                }
            }
        }

        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Zone"))
            {
                var zoneName = collision.gameObject.GetComponent<ZoneObject>().GetZoneName();
                if (collidingZones.Contains(zoneName))
                {
                    collidingZones.Remove(zoneName);
                }
            }
        }

    }
}