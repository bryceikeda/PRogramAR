using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/ZonePropertyList"), System.Serializable]
    public class ZonePropertyList : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public Dictionary<int, (string, Color)> zoneProperties = new Dictionary<int, (string, Color)>() {
            { 1, ("Red", new Color(0.7607843f, 0.211764708f, 0.08627451f, 0.698039234f)) },
            { 2, ("Blue", new Color(0.1177867f, 0.09211462f, 0.8490566f, 0.698039234f)) },
            { 3, ("Yellow", new Color(0.882352948f, 0.694117665f, 0.172549024f, 0.698039234f)) },
            { 4, ("Green", new Color(0.266666681f, 0.7411765f, 0.196078435f, 0.698039234f)) },
            { 5, ("Purple", new Color(0.549019635f, 0.478431374f, 0.9019608f, 0.698039234f)) },
            { 6, ("Pink", new Color(1f, 0f,0.847058833f,  0.698039234f)) },
            { 7, ("Grey", new Color(0.2509804f, 0.4509804f, 0.619607866f, 0.698039234f)) },
            { 8, ("Brown", new Color(0.5803922f, 0.349019617f, 0.09803922f, 0.698039234f)) },
            { 9, ("Red", new Color(0.7607843f, 0.211764708f, 0.08627451f, 0.698039234f)) }
        };



    }
}
