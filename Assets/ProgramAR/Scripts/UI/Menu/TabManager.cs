using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Menu
{
    public class TabManager : ToggleCollection
    {
        public List<GameObject> pages;

        protected override void Start()
        {
/*            foreach (GameObject obj in pages)
            {
                obj.SetActive(false); 
            }*/
            base.Start(); 
        }

        protected override void OnSelection(int index)
        {
            base.OnSelection(index);
            if (PreviousIndex != -1)
            {
                pages[PreviousIndex].SetActive(false);
            }
            pages[index].SetActive(true);
        }

        public void ChangeTab(int index)
        {
            OnSelection(index);
        }
    }
}
