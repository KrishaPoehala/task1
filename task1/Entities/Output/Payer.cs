namespace task1.Entities;
// "name": "string",
//      "payment": "decimal",
//      "date": "date",
//      "account_number": "long"
public class Payer
{
    public string Name { get; set; }
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }

    public Payer(Transaction t)
    {
        Name = t.FirstName;
        Date = t.Date;
        Payment = t.Payment;
        AccountNumber = t.AccountNumber;
    }

    public Payer()
    {

    }
}
