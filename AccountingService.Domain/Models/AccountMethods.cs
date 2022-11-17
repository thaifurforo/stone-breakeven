using Microsoft.AspNetCore.Http;

namespace AccountingService.Domain.Models;

public partial class Account
{
    public string GetAccountNumber()
    {
        var numBase = Id;
        string numberBeforeDigit = numBase.ToString("D6");

        int[] multipliers = { 7, 6, 5, 4, 3, 2 };
        char[] listOfNumbers = numberBeforeDigit.ToCharArray();

        int accountDigit = 0;

        for (int i = 0; i <= 5; i++)
        {
            accountDigit += multipliers[i] * int.Parse(listOfNumbers[i].ToString());
        }

        accountDigit %= 11;

        if (accountDigit is 1 or >= 10)
            accountDigit = 0;

        Number = numberBeforeDigit + '-' + accountDigit;

        return Number;
    }
    
    public void DeactivateAccount()
    {
        var genericCurrentDate = DateTime.Now;

        if (genericCurrentDate < OpeningDate)
        {
            throw new BadHttpRequestException($"Invalid request: Closing date must be equal or greater than opening date. Please, try again.");
        }
        else if (Balance != 0)
        {
            throw new BadHttpRequestException($"Invalid request: It's impossible to deactivate an account whose balance is different from 0. Please, try again.");
        }
        else if (!IsActive)
        {
            throw new BadHttpRequestException($"Invalid request: This account is already deactivated.");
        }
        else
        {
            IsActive = false;
            ClosingDate = genericCurrentDate;    
        }
        
        
    }

}

