namespace ToDoAppSharedModels.Results
{
    public enum LoginResult
    {
        Success,
        SuccessShouldUpdatePassword,
        UnknownError,
        AccountNotFound,
        WrongPassword
    }
}
