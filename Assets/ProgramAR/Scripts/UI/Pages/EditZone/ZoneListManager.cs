using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ZoneListManager : MonoBehaviour, IDeleteZoneResponse
    {
        public EditZoneEvent editZone;
        public DeleteZoneEvent deleteZone;
        public SpawnZoneEvent spawnZone;

        [SerializeField] GameObject createZoneButton;
        [SerializeField] ZonePropertyList zoneProperties;
        [SerializeField] ZoneButtonGroupFactory zoneButtonFactory;

        private List<int> zoneIndex;
        private Dictionary<int, GameObject> buttonObjects = new Dictionary<int, GameObject>();
        private GridObjectCollection gridComponent;

        [SerializeField] ZoneTransformList zoneTransforms;
        private bool loaded = false;
        
        private void OnValidate()
        {
            zoneIndex = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            gridComponent = GetComponent<GridObjectCollection>();
        }

        public void Start()
        {
            Load();
        }

        public void SpawnZone()
        {
            if (zoneIndex.Count != 0)
            {
                int selectedZone = zoneIndex[0];
                ZoneButtonGroup buttonGroup = zoneButtonFactory.GetInstance();
                buttonGroup.transform.parent = transform;
                buttonGroup.transform.localRotation = Quaternion.identity; 

                // Set the zone number, label and color
                if (zoneProperties.zoneProperties.TryGetValue(selectedZone, out (string, Color) value))
                {
                    string label = "Zone " + selectedZone + "\n" + value.Item1;
                    buttonGroup.SetZoneButtonGroup(selectedZone, label, value.Item2);
                }
                else
                {
                    Debug.LogError("[ZoneListManager]: No zone property for selectedZone " + selectedZone);
                    return;
                }

                buttonGroup.transform.SetSiblingIndex(selectedZone - 1);

                buttonObjects.Add(selectedZone, buttonGroup.gameObject);

                spawnZone.Raise(selectedZone);
                zoneIndex.RemoveAt(0);
            }

            createZoneButton.SetActive(zoneIndex.Count != 0);
            gridComponent.UpdateCollection();
        }
        public void SpawnZone(int index)
        {
            if (zoneIndex.Count != 0)
            {
                ZoneButtonGroup buttonGroup = zoneButtonFactory.GetInstance();
                buttonGroup.transform.parent = transform;
                buttonGroup.transform.localRotation = Quaternion.identity;
                // Set the zone number, label and color
                if (zoneProperties.zoneProperties.TryGetValue(index, out (string, Color) value))
                {
                    string label = "Zone " + index + "\n" + value.Item1;
                    buttonGroup.SetZoneButtonGroup(index, label, value.Item2);
                }
                else
                {
                    Debug.LogError("[ZoneListManager]: No zone property for selectedZone " + index);
                    return;
                }

                buttonGroup.transform.SetSiblingIndex(index - 1);

                buttonObjects.Add(index, buttonGroup.gameObject);

                zoneIndex.Remove(index);
            }

            createZoneButton.SetActive(zoneIndex.Count != 0);
            gridComponent.UpdateCollection();
        }


        public void OnDeleteZoneEvent(int selectedZone)
        {
            if (buttonObjects.TryGetValue(selectedZone, out GameObject value))
            {
                value.transform.SetAsLastSibling();

                Destroy(value);
                buttonObjects.Remove(selectedZone);

                zoneIndex.Add(selectedZone);
                zoneIndex.Sort();
            }
            else
            {
                Debug.LogWarning("[ZoneListManager]: No button group to delete for selectedZone " + selectedZone);
            }

            createZoneButton.SetActive(zoneIndex.Count != 0);
            gridComponent.UpdateCollection();
        }

        private void Load()
        {
            if (loaded == false)
            {
                foreach (int index in zoneTransforms.indexes)
                {
                    SpawnZone(index);
                }
                loaded = true;
                createZoneButton.transform.SetAsLastSibling();
                gridComponent.UpdateCollection();
            }
        }

        private void OnEnable()
        {
            deleteZone.RegisterListener(this);
            createZoneButton.GetComponent<Interactable>().OnClick.AddListener(SpawnZone);
        }

        private void OnDisable()
        {
            if (createZoneButton != null)
            {
                createZoneButton.GetComponent<Interactable>().OnClick.RemoveListener(SpawnZone);
            }
            deleteZone.UnregisterListener(this); 
        }
    }
}
