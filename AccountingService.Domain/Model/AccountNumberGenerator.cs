namespace AccountingService.Domain.Model;

public class AccountNumberGenerator
{
    private static string Number { get; set; }

    public static int GetAccountNumber()
    {
        string NumberBeforeDigit = Number.Split('-')[0];

        int[] multipliers = { 7, 6, 5, 4, 3, 2 };
        
        int AccountDigit;
        
        for (int i = 0; i <= 5; i++)
        {
            AccountDigit += multipliers[i] * 
        }
        return Number;
    }
}