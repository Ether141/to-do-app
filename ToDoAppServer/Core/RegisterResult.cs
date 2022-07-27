namespace ToDoAppServer.Core
{
    public enum RegisterResult
    {
        Success,
        UnknownError,
        AccountWithEmailAlreadyExists,
        AccountWithNicknameAlreadyExists,
        WrongEmail,
        WrongPassword,
        WrongNickname,
        PasswordsNotSame
    }
}
