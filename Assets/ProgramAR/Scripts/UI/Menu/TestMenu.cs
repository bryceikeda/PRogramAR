using UnityEngine;

namespace ProgramAR
{
    namespace Menu
    {
        public class TestMenu : MonoBehaviour
        {
            public PageController pageController;

#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    pageController.TurnPageOn(PageType.EditZones);
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    pageController.TurnPageOff(PageType.EditZones);
                }
                if (Input.GetKeyUp(KeyCode.H))
                {
                    pageController.TurnPageOff(PageType.EditZones, PageType.CreateRules);
                }
                if (Input.GetKeyUp(KeyCode.J))
                {
                    pageController.TurnPageOff(PageType.EditZones, PageType.CreateRules, true);
                }
            }
#endif
        }
    }
}
