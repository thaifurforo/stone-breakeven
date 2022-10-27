namespace AccountingService.Domain.Model;

public class AccountNumberGenerator
{
    private static string Number { get; set; }

    public static string GetAccountNumber()
    {
        string numberBeforeDigit = Number.Split('-')[0];

        int[] multipliers = { 7, 6, 5, 4, 3, 2 };
        string[] listOfNumbers = numberBeforeDigit.Split();
        
        int accountDigit = 0;
        
        for (int i = 0; i <= 5; i++)
        {
            accountDigit += multipliers[i] * int.Parse(listOfNumbers[i]);
        }

        if (accountDigit == 1)
            accountDigit = 0;

        Number = ((numberBeforeDigit) + 1).ToString() + '-' + accountDigit; 
        
        return Number;
    }
}