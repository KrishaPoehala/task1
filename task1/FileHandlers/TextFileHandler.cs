using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;
using System.Transactions;
using task1.Helpers;

namespace task1.FileHandlers;

public class TextFileHandler
{
    public int InvalidLinesCount { get; private set; }
    public static bool IsFileInvalid { get; private set; }

    private readonly TextSource<Entities.Transaction> _textSource;
    private readonly MemoryDestination<Entities.Transaction> _dest;
    public TextFileHandler(string path)
    {
        _textSource = new TextSource<Entities.Transaction>
        {
            Uri = path,
            ParseLineFunc = (line, _) => Handle(line)
        };

        _dest = new MemoryDestination<Entities.Transaction>();
        _textSource.LinkTo(_dest);
    }

    public async Task<ICollection<Entities.Transaction>> ExecuteAsync()
    {
        await Network.ExecuteAsync(_textSource);
        return _dest.Data;
    }

    public Entities.Transaction? Handle(string line)
    {
        try
        {
            var tokens = line.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            var transaction = new Entities.Transaction()
            {
                FirstName = tokens[0],
                LastName = tokens[1],
                City = tokens[2][1..], //ignore " and etc
                Payment = decimal.Parse(tokens[5].Trim().Replace(".", ",")),
                Date = DateTime.ParseExact(DateTimeHelper.Reverse(tokens[6]), "MM/dd/yyyy", null),
                AccountNumber = AccountHelper.Parse(tokens[7]),
                Service = tokens[8],
            };
            return transaction;
        }
        catch
        {
            IsFileInvalid = true;
            ++InvalidLinesCount;
            return null;
        }
    }
}
