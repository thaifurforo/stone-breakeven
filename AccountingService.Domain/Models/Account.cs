using System;
using System.Text.Json.Serialization;
using DateOnlyTimeOnly.AspNet.Converters;

namespace AccountingService.Domain.Model;

public partial class Account
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Agency { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly OpeningDate { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly? ClosingDate { get; set; }

    public string Document { get; set; }

    public Account(string document, string agency, DateOnly? closingDate = null, bool status = true)

    {
        Id = GetNextId();
        Agency = agency;
        Number = GetAccountNumber(Id);
        Amount = 0;
        Status = status;
        OpeningDate = DateOnly.FromDateTime(DateTime.Now);
        ClosingDate = closingDate;
        Document = document;
    }
    
}
    