using UnityEngine;
using ProgramAR.Pages; 
using TMPro;
using System.Collections;

namespace RuleProcessor
{
    public class HeadTextUpdate : MonoBehaviour, IPlanStatusResponse
    {
        TextMeshPro headText;
        [SerializeField] PlanStatusEvent planStatusEvent;

        // Start is called before the first frame update
        public void OnValidate()
        {
            headText = GetComponent<TextMeshPro>();
        }

        public void OnPlanStatusEvent(string status)
        {
            if (headText.text.Contains("Error"))
            {
                StartCoroutine(ShowNext(status));
            }
            else
            {
                headText.text = status;
            }
        }

        IEnumerator ShowNext(string status)
        {
            yield return new WaitForSeconds(5);
            headText.text = status; 
        }

        void OnEnable()
        {            
            planStatusEvent.RegisterListener(this);
        }

        void OnDisable()
        {
            planStatusEvent.UnregisterListener(this);
        }
    }
}
