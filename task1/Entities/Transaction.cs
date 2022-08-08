namespace task1.Entities;

public record Transaction
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }
    public string Service { get; set; }
    public Transaction()
    {

    }
}
