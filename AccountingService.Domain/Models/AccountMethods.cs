namespace AccountingService.Domain.Model;


public partial class Account
{
    public static string GetAccountNumber(int id)
    {
        string numberBeforeDigit = id.ToString("D6");

        int[] multipliers = { 7, 6, 5, 4, 3, 2 };
        char[] listOfNumbers = numberBeforeDigit.ToCharArray();

        int accountDigit = 0;

        for (int i = 0; i <= 5; i++)
        {
            accountDigit += multipliers[i] * int.Parse(listOfNumbers[i].ToString());
        }

        if (accountDigit == 1)
            accountDigit = 0;

        Number = (numberBeforeDigit).ToString() + '-' + accountDigit;

        return Number;
    }
    
    public static int GetNextId()
    {
        Id++;
        return Id;
    }

}
