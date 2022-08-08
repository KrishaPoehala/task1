﻿using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;
using System.Transactions;
using task1.Helpers;

namespace task1.FileHandlers;

public class TextFileHandler :BaseFileHandler<Entities.Transaction>
{
    public TextFileHandler(string path)
    {
        _source = new TextSource<Entities.Transaction>
        {
            Uri = path,
            ParseLineFunc = (line, _) => HandleLine(line)
        };

        _dest = new MemoryDestination<Entities.Transaction>();
        _source.LinkTo(_dest);
    }

    private Entities.Transaction? HandleLine(string line)
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
