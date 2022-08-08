namespace task1.Entities;
//[{
//  "city": "string",
//  "services": [{
//    "name": "string",
//    "payers": [{
//      "name": "string",
//      "payment": "decimal",
//      "date": "date",
//      "account_number": "long"
//    }],
//    "total": "decimal"
//  }],
//  "total": "decimal"
//}]

public class OutputType
{
    public string City { get; set; }
    public IList<Service> Services { get; set; } = new List<Service>();
    public decimal Total { get; set; }
}
