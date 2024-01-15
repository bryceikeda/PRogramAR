using UnityEngine;

namespace ProgramAR
{
    namespace ButtonHelpers
    {
        public class ButtonColorHelper : MonoBehaviour
        {
            public Renderer backPlateToggleState;
            public Renderer backPlate;

            public void SetColor(Color color)
            {
                backPlateToggleState.material.color = color;
                backPlate.material.color = color;
            }
        }

    }
}
