public class UserLevelState
{
    public event Action OnChange;

    public int LevelId { get; private set; }
    public int CurrentXP { get; private set; }

    public void SetUserLevel(int levelId, int currentXP)
    {
        LevelId = levelId;
        CurrentXP = currentXP;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
