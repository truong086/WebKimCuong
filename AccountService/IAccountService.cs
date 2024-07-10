using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IAccountService
    {
        #region Account
        Task<AccountDTO> Login(Login login);
        Task<AccountDTO> AddAccount(AccountDTO accountDTO);
        Task<AccountDTO> EditAccount(int id, AccountDTO accountDTO);
        Task<string> DeleteAccount(IList<string> id);
        Task<string> ConvertToHS256(string value);
        Task<string> DecryptHS256(string encodedValue);
        Task<string> Action(string code);
        Task<AccountDTO> Logout(string name);
        Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20);
        Task<PayLoad<List<AccountDTO>>> FindOne(IList<string> id);
        Task<TokenMessage> checkToken(string token);
        #endregion
        #region Role
        Task<PayLoad<object>> findAllRole();
        #endregion
    }
}
