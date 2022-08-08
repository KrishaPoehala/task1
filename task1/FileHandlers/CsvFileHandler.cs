using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;
using ETLBox.DataFlow.Transformations;
using System.Dynamic;
using task1.Entities;
using task1.Helpers;

namespace task1.FileHandlers;

public class CsvFileHandler : BaseFileHandler<ExpandoObject>
{
    public CsvFileHandler(string path)
    {
        _dest = new MemoryDestination<Entities.Transaction>();
        _source = new CsvSource<ExpandoObject>()
        {
            Uri = path,
        };

        var row = new RowTransformation<ExpandoObject,Transaction>(data => Handle(data));
        _source.LinkTo(row);
        row.LinkTo(_dest);
    }

    public Transaction Handle(ExpandoObject data)
    {
        dynamic d = data;
        try
        {
            var transaction = new Entities.Transaction()
            {
                FirstName = d.first_name,
                LastName = d.last_name,
                City = d.address[1..], //ignore " and etc
                Payment = decimal.Parse(d.payment.Trim().Replace(".", ",")),
                Date = DateTime.ParseExact(DateTimeHelper.Reverse(d.date), "MM/dd/yyyy", null),
                AccountNumber = AccountHelper.Parse(d.account_number),
                Service = d.service,
            };

            return transaction;
        }
        catch (Exception)
        {
            ++_invalidLinesCount;
            return null;
        }
    }
}
