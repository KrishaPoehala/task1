namespace task1.FileWatcher;

public class FileWatcher : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly FileProcessor.IFileProccessor _fileProcessor;
    public FileWatcher(string pathToListen,FileProcessor.IFileProccessor fileProccessor)
    {
        _fileProcessor = fileProccessor;
        _watcher = new(pathToListen);
        _watcher.NotifyFilter = NotifyFilters.Attributes
                                      | NotifyFilters.CreationTime
                                      | NotifyFilters.DirectoryName
                                      | NotifyFilters.FileName
                                      | NotifyFilters.LastAccess
                                      | NotifyFilters.LastWrite
                                      | NotifyFilters.Security
                                      | NotifyFilters.Size;
        _watcher.EnableRaisingEvents = true;
        _watcher.Created += OnCreated;
        _watcher.Filter = "*.*";//prop filter does not support multipule file filters
    }

    public void Dispose()
    {
        _watcher.Dispose();
    }

    async void OnCreated(object sender, FileSystemEventArgs e)
    {
        await OnCreatedAsync(e);
    }

    private async Task OnCreatedAsync(FileSystemEventArgs e)
    {
        await _fileProcessor.ProccessFile(e.FullPath);
    }
}
