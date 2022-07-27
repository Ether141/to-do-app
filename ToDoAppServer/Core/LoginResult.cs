namespace ToDoAppServer.Core
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
