
using UnityEngine;
using ProgramAR.Pages;
using System.IO;
using System.Collections.Generic;
using ProgramAR.Variables; 

/// <summary>
///
/// </summary>

public class GameData : MonoBehaviour
{
    private string saveTAPPath;
    private string saveZonesPath;

    [SerializeField] IntegerVariable participantNumber;
    [SerializeField] IntegerVariable taskNumber;
    [SerializeField] bool loadProfile = true;

    [SerializeField] TriggerActionRules triggerActionRules;
    [SerializeField] ZoneObjectRuntimeDictionary zones;
    [SerializeField] SpawnZoneEvent spawnZoneEvent;

    [SerializeField] ZoneTransformList zoneTransforms; 

    private void Awake()
    {
        saveTAPPath = Application.persistentDataPath + "/" + participantNumber.Value + "_" + taskNumber.Value + "_TAP" + ".json";
        saveZonesPath = Application.persistentDataPath + "/" + participantNumber.Value + "_" + taskNumber.Value + "_Zones" + ".json";

        if (loadProfile)
        {
            if (File.Exists(saveZonesPath))
            {
                var json = File.ReadAllText(saveZonesPath);
                JsonUtility.FromJsonOverwrite(json, zoneTransforms);
            }

            if (File.Exists(saveTAPPath))
            {
                var json = File.ReadAllText(saveTAPPath);
                JsonUtility.FromJsonOverwrite(json, triggerActionRules);
            }
        }
    }

    private void OnApplicationQuit()
    {
        zoneTransforms.Clear();
        foreach (KeyValuePair<int, ZoneObject> pair in zones.Items)
        {
            zoneTransforms.Add(pair.Key, new SerializedTransform(pair.Value.transform));
        }

        string saveTAPText = JsonUtility.ToJson(triggerActionRules);
        File.WriteAllText(saveTAPPath, saveTAPText);

        string saveZoneText = JsonUtility.ToJson(zoneTransforms);
        File.WriteAllText(saveZonesPath, saveZoneText);
        
        triggerActionRules.pairs.Clear();
        zones.Items.Clear();
        zoneTransforms.Clear();
    }
}
