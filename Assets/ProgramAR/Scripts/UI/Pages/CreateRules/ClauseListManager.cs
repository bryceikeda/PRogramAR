using UnityEngine;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using ProgramAR.Events;
using ProgramAR.Variables;
using ProgramAR.Menu;

namespace ProgramAR.Pages
{
    [RequireComponent(typeof(GridObjectCollection))]
    public class ClauseListManager : MonoBehaviour, IDeleteClauseResponse, IHideClauseListResponse, IShowClauseListResponse
    {
        [SerializeField] DeleteClauseEvent deleteClauseEvent;
        [SerializeField] ShowClauseListEvent showClauseListEvent;
        [SerializeField] HideClauseListEvent hideClauseListEvent;
        [SerializeField] ToggleSaveVisibilityEvent toggleSaveVisibilityEvent;

        [SerializeField] ClauseRuntimeSet clauseRuntimeSet;
        [SerializeField] ClauseButtonGroupFactory clauseButtonGroupFactory;
        [SerializeField] TriggerActionRules triggerActionRules; 
        [SerializeField] GameObject createClauseButton;
        [SerializeField] BoolVariable defaultClauseAdded;
        [SerializeField] BoolVariable otherDefaultClauseAdded;

        [SerializeField] GameObject ruleTypeBackButton; 

        private List<ClauseButtonGroup> clauseButtons = new List<ClauseButtonGroup>();
        private GridObjectCollection gridObjectCollection;

        private void OnValidate()
        {
            if (gridObjectCollection == null)
            {
                gridObjectCollection = GetComponent<GridObjectCollection>();
            }
        }

        public void OnHideClauseListEvent()
        {
            foreach (ClauseButtonGroup button in clauseButtons)
            {
                button.gameObject.SetActive(false);
            }
            toggleSaveVisibilityEvent.Raise(false, "OnHideClauseListEvent");
            ruleTypeBackButton.SetActive(false);
            createClauseButton.SetActive(false);
        }

        public void OnDeleteClauseEvent(int clauseIndex)
        {
            if (clauseIndex == 0)
            {
                defaultClauseAdded.SetValue(false);
                toggleSaveVisibilityEvent.Raise(false, "OnHideClauseListEvent");
            }
            clauseRuntimeSet.Items.RemoveAt(clauseIndex);
            clauseButtons.RemoveAt(clauseIndex);

            var obj = transform.GetChild(clauseIndex);


            createClauseButton.SetActive(true);
            OnShowClauseListEvent();
            obj.SetAsLastSibling();
            Destroy(obj.gameObject);
            gridObjectCollection.UpdateCollection();
        }

        public void OnShowClauseListEvent()
        {
            if (defaultClauseAdded.Value && otherDefaultClauseAdded.Value)
            {
                toggleSaveVisibilityEvent.Raise(true, "ClauseListManager");
            }

            // Create a button if we have extra rule
            while (clauseButtons.Count < clauseRuntimeSet.Items.Count)
            {
                ClauseButtonGroup button = clauseButtonGroupFactory.GetInstance();
                button.gameObject.transform.parent = transform; 
                button.gameObject.transform.localRotation = Quaternion.identity;
                clauseButtons.Add(button);
            }

            //  Give buttons their descriptions
            for (int clauseNum = 0; clauseNum < clauseRuntimeSet.Items.Count; clauseNum++)
            {
                clauseButtons[clauseNum].gameObject.SetActive(true);
                clauseButtons[clauseNum].SetProperties(clauseRuntimeSet.Items[clauseNum].GetClauseName(), clauseNum);
            }
            createClauseButton.SetActive(clauseRuntimeSet.Items.Count != 5);
            createClauseButton.transform.SetAsLastSibling();

            if (clauseRuntimeSet.Items.Count == 0)
            {
                for(int i = clauseButtons.Count - 1; i >= 0; i--)
                {
                    GameObject obj = clauseButtons[i].gameObject;
                    obj.transform.SetAsLastSibling();
                    clauseButtons.Remove(clauseButtons[i]);
                    Destroy(obj);
                }
                createClauseButton.transform.SetAsFirstSibling();
            }

            gridObjectCollection.UpdateCollection();
        }


        private void OnEnable()
        {
            deleteClauseEvent.RegisterListener(this);
            showClauseListEvent.RegisterListener(this);
            hideClauseListEvent.RegisterListener(this);

        }

        private void OnDisable()
        {
            deleteClauseEvent.UnregisterListener(this);
            showClauseListEvent.UnregisterListener(this);
            hideClauseListEvent.UnregisterListener(this);
        }
    }
}