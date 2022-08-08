using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;

namespace task1.FileHandlers;

public class BaseFileHandler<TSource>
{
    protected int _invalidLinesCount;
    protected DataFlowStreamSource<TSource> _source;
    protected MemoryDestination<Entities.Transaction> _dest;
    public BaseFileHandler()
    {

    }
    public async Task<Entities.FileInfo> ExecuteAsync()
    {
        await Network.ExecuteAsync(_source);
        return new Entities.FileInfo()
        {
            Transactions = _dest.Data,
            InvalidLinesCount = _invalidLinesCount,
            FullPath = _source.Uri,
        };
    }
}
