using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class ZoneSpawner : MonoBehaviour, IToggleZoneResponse, ISpawnZoneResponse, IDeleteZoneResponse, IEditZoneResponse
    {
        [SerializeField] ZoneObjectFactory factory;
        [SerializeField] ZonePropertyList propertyList;
        [SerializeField] ToggleZoneEvent toggleZoneEvent;
        [SerializeField] SpawnZoneEvent spawnZoneEvent;
        [SerializeField] DeleteZoneEvent deleteZoneEvent;
        [SerializeField] EditZoneEvent editZoneEvent;
        [SerializeField] ZoneObjectRuntimeDictionary zones;
        [SerializeField] ZoneTransformList zoneTransforms;
        private bool loaded = false;
        [SerializeField] Transform zoneSpawnPoint; 
        private void Start()
        {
            Load();
        }

        public void OnSpawnZoneEvent(int selectedZone, SerializedTransform t = null)
        {
            ZoneObject obj = factory.GetInstance();

            obj.transform.parent = transform;
            obj.transform.position = zoneSpawnPoint.position;
            obj.transform.rotation = zoneSpawnPoint.rotation; 
            if (t != null)
            {
                obj.transform.localPosition = new Vector3(t._position[0], t._position[1], t._position[2]);
                obj.transform.localRotation = new Quaternion(t._rotation[0], t._rotation[1], t._rotation[2], t._rotation[3]);
                obj.transform.localScale = new Vector3(t._scale[0], t._scale[1], t._scale[2]);
            }

            if (propertyList.zoneProperties.TryGetValue(selectedZone, out (string, Color) value))
            {
                obj.SetZoneColor(value.Item2);
            }
            else
            {
                Debug.LogWarning("[Zone Spawner]: No property type for selectedZone " + selectedZone);
            }

            obj.SetZoneName("Zone " + selectedZone.ToString()); 

            zones.Add(selectedZone, obj);
        }

        public void OnDeleteZoneEvent(int selectedZone)
        {
            ZoneObject obj; 
            if (zones.Items.TryGetValue(selectedZone, out ZoneObject zone))
            {
                obj = zone;
                zones.Items.Remove(selectedZone);
                Destroy(obj.gameObject);
            }
            else
            {
                Debug.LogWarning("[Zone Spawner]: No zone to delete for selectedZone " + selectedZone);
            }
        }

        public void OnEditZoneEvent(int selectedZone)
        {
            if (zones.Items.TryGetValue(selectedZone, out ZoneObject zone))
            {
                zone.ToggleInteraction();
            }
            else
            {
                Debug.LogWarning("[Zone Spawner]: No zone to delete for selectedZone " + selectedZone);
            }
        }

        public void OnToggleZoneEvent(int selectedZone, bool isToggled)
        {
            if (zones.Items.TryGetValue(selectedZone, out ZoneObject zone))
            {
                if (zone.gameObject.activeSelf)
                {
                    zone.DisableZoneInteraction();
                }
                zone.gameObject.SetActive(isToggled);
            }
            else
            {
                Debug.LogWarning("[Zone Spawner]: No zone to hide for selectedZone " + selectedZone);
            }
        }

        private void Load()
        {
            if (loaded == false)
            {
                for (int i = 0; i < zoneTransforms.indexes.Count; i++)
                {
                    OnSpawnZoneEvent(zoneTransforms.indexes[i], zoneTransforms.transforms[i]);
                }
                loaded = true; 
            }
        }

        private void OnEnable()
        {
            toggleZoneEvent.RegisterListener(this);
            spawnZoneEvent.RegisterListener(this);
            deleteZoneEvent.RegisterListener(this);
            editZoneEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            toggleZoneEvent.UnregisterListener(this);
            spawnZoneEvent.UnregisterListener(this);
            deleteZoneEvent.UnregisterListener(this);
            editZoneEvent.UnregisterListener(this);
        }
    }
}
