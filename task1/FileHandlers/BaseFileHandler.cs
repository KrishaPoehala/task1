using ETLBox.DataFlow;
using ETLBox.DataFlow.Connectors;

namespace task1.FileHandlers;

public abstract class BaseFileHandler
{
    protected int _invalidLinesCount;
    protected MemoryDestination<Entities.Transaction> _dest;
    public BaseFileHandler()
    {

    }

    public abstract Task<Entities.FileInfo> ExecuteAsync();
}
