

/// <summary>
///
/// </summary>

namespace ProgramAR.Events
{
    public interface IToggleSaveVisibilityResponse
    {
        void OnToggleSaveVisibilityEvent(bool visibility, string saveOrigin);
        void OnToggleSaveVisibilityEvent(bool visibility);
    }
}
