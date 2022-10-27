using System;

namespace AccountingService.Domain.Exception;

[Serializable]
public class InvalidDocumentException : System.Exception
{
    public InvalidDocumentException() : base("Invalid document number")
    {}
}