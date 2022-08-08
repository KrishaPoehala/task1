namespace task1.Entities;
//"name": "string",
//    "payers": [{
//      "name": "string",
//      "payment": "decimal",
//      "date": "date",
//      "account_number": "long"
//    }],
public class Service
{
    public string Name { get; set; }
    public IList<Payer> Payers { get; set; } = new List<Payer>();
    public decimal Total { get; set; }
}
