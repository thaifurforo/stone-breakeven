using AccountingService.Domain.Model;

namespace AccountingService.Api.Contracts.v1.Response;

public class AccountList
{
    public static List<Account> Accounts = new();

    public static bool IsEmptyAccounts()
    {
        return Accounts.Count == 0;
    }
}