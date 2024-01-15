using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class OnClickIconTextChange : MonoBehaviour
    {
        [SerializeField] ButtonConfigHelper config;
        bool active = false;
        public string onText;
        public string offText;
        public string onIcon;
        public string offIcon; 
        public void OnClick()
        {
            if (active == false)
            {
                active = true;
                config.MainLabelText = onText;
                config.SetQuadIconByName(onIcon);
            }
            else
            {
                active = false;
                config.MainLabelText = offText;
                config.SetQuadIconByName(offIcon);

            }
        }
    }
}
