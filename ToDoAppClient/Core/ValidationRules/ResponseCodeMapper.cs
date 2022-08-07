using RestSharp;
using ToDoAppClient.Resources.Strings;
using ToDoAppSharedModels.Results;

namespace ToDoAppClient.Core.ValidationRules
{
    public static class ResponseCodeMapper
    {
        public static string MapResponseStatus(ResponseStatus status) => status switch
        {
            ResponseStatus.Completed => Resource.success,
            _ => Resource.cannotConnect
        };

        public static string MapRegisterResult(RegisterResult result) => result switch
        {
            RegisterResult.Success => Resource.success,
            RegisterResult.UnknownError => Resource.error,
            RegisterResult.AccountWithEmailAlreadyExists => Resource.accountWithEmailAlreadyExists,
            RegisterResult.AccountWithNicknameAlreadyExists => Resource.accountWithNicknameAlreadyExists,
            RegisterResult.WrongEmail => Resource.wrongEmail,
            RegisterResult.WrongPassword => Resource.wrongPassword,
            RegisterResult.WrongNickname => Resource.wrongNickname,
            RegisterResult.PasswordsNotSame => Resource.passwordsAreNotSame,
            _ => Resource.error
        };

        public static string MapLoginResult(LoginResult result) => result switch
        {
            LoginResult.Success => Resource.success,
            LoginResult.SuccessShouldUpdatePassword => Resource.success,
            LoginResult.UnknownError => Resource.error,
            LoginResult.AccountNotFound => Resource.wrongNickname,
            LoginResult.WrongPassword => Resource.wrongPassword,
            _ => Resource.error
        };
    }
}
