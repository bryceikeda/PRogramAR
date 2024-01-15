using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class ClauseButtonGroup : MonoBehaviour
    {
        [SerializeField] DeleteClauseEvent deleteClauseEvent;
        [SerializeField] EditClauseEvent editClauseEvent;
        [SerializeField] HideClauseListEvent hideClauseListEvent;

        [SerializeField] ButtonConfigHelper ruleButtonConfig;
        [SerializeField] ButtonConfigHelper deleteButtonConfig;
        public int selectedClause;

        public void SetProperties(string name, int selectedClause)
        {
            this.selectedClause = selectedClause;
            ruleButtonConfig.MainLabelText = name;
        }

        public void OnDeleteClauseEvent()
        {
            deleteClauseEvent.Raise(selectedClause);
        }

        public void OnEditClauseEvent()
        {
            hideClauseListEvent.Raise();
            editClauseEvent.Raise(selectedClause);
        }

        private void OnEnable()
        {
            ruleButtonConfig.OnClick.AddListener(OnEditClauseEvent);
            deleteButtonConfig.OnClick.AddListener(OnDeleteClauseEvent);

        }
        private void OnDisable()
        {
            if (ruleButtonConfig != null)
            {
                ruleButtonConfig.OnClick.RemoveListener(OnEditClauseEvent);
            }
            if (deleteButtonConfig != null)
            {
                deleteButtonConfig.OnClick.RemoveListener(OnDeleteClauseEvent);
            }

        }
    }
}