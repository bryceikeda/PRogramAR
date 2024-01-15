using TMPro;
using UnityEngine;

namespace ProgramAR
{
    namespace ButtonHelpers
    {
        public class ButtonTextHelper : MonoBehaviour
        {
            public TextMeshPro buttonTextReference;

            public void SetText(string text)
            {
                buttonTextReference.text = text;
            }
        }
    }
}
