namespace CesiZen.Domain.BusinessResult;

public static class BreathExerciseInfos
{
    #region Client Messages
    public static Info InsertionSucceeded => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_INSERTION_SUCCESS"), "L'exercice de respiration'"));
    public static Info UpdateSucceeded => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "L'exercice de respiration"));
    public static Info DeleteCompleted => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "L'exercice de respiration"));
    #endregion

    #region Log Messages
    public static Info LogInsertionSucceeded(string identifier) => new(InfoType.InsertionSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_INSERT_SUCCESS"), "Breath exercise", identifier));
    public static Info LogUpdateSucceeded(string id) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_SUCCESS"), "Breath exercise", id));
    public static Info LogUpdateProperty(string property) => new(InfoType.UpdateSucceeded, string.Format(Message.GetResource("InfoMessages", "LOG_UPDATE_PROPERTY"), "Breath exercise", property));
    public static Info LogDeleteCompleted(string id) => new(InfoType.DeleteCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_DELETION_SUCCESS"), "Breath exercise", id));
    public static Info LogCollectCompleted(string argument) => new(InfoType.CollectCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_COLLECT_COMPLETED"), "Breath exercise", argument));
    public static Info LogFormatCompleted(string property) => new(InfoType.FormatCompleted, string.Format(Message.GetResource("InfoMessages", "LOG_FORMAT_COMPLETED"), "Breath exercise", property));
    #endregion
}
