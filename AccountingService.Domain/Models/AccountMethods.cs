

using Microsoft.AspNetCore.Http;

namespace AccountingService.Domain.Models;


public partial class Account
{
    public string GetAccountNumber()
    {
        string numberBeforeDigit = Id.ToString("D6");

        int[] multipliers = { 7, 6, 5, 4, 3, 2 };
        char[] listOfNumbers = numberBeforeDigit.ToCharArray();

        int accountDigit = 0;

        for (int i = 0; i <= 5; i++)
        {
            accountDigit += multipliers[i] * int.Parse(listOfNumbers[i].ToString());
        }

        accountDigit %= 11;

        if (accountDigit == 1)
            accountDigit = 0;

        Number = numberBeforeDigit + '-' + accountDigit;

        return Number;
    }
    
    public void DeactivateAccount()
    {
        var genericCurrentDate = DateTime.Now;

        if (ClosingDate < OpeningDate)
        {
            throw new BadHttpRequestException($"Invalid request: Closing date must be equal or greater than opening date. Please, try again.");
        }
        else if (Balance != 0)
        {
            throw new BadHttpRequestException($"Invalid request: It's impossible to deactivate an account whose balance is different from 0. Please, try again.");
        }
        else
        {
            IsActive = false;
            ClosingDate = genericCurrentDate;    
        }
        
        
    }

}

