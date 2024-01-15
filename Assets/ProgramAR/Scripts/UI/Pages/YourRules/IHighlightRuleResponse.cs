namespace ProgramAR.Pages
{
    public interface IHighlightRuleResponse
    {
        void OnHighlightRuleEvent(int index, bool triggerIsValid, bool actionIsValid); 
    }
}
