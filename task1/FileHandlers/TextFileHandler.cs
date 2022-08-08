﻿using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;
using System.Transactions;
using task1.Helpers;

namespace task1.FileHandlers;

public class TextFileHandler
{
    private int _invalidLinesCount;
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

    public async Task<Entities.FileInfo> ExecuteAsync()
    {
        await Network.ExecuteAsync(_textSource);
        return new Entities.FileInfo() 
        { 
            Transactions = _dest.Data, 
            InvalidLinesCount = _invalidLinesCount,
            FullPath = _textSource.Uri,
        };
    }

    private Entities.Transaction? Handle(string line)
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
            ++_invalidLinesCount;
            return null;
        }
    }
}
