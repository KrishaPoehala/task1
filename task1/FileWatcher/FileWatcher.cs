using Newtonsoft.Json;
using System.Configuration;
using System.Text.Json.Serialization;
using task1.FileHandlers;
using task1.Helpers;

namespace task1.FileWatcher;

public class FileWatcher
{
    private readonly FileSystemWatcher _watcher;
    public FileWatcher()
    {
        var pathToListen = ConfigurationManager.AppSettings.Get("A");
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
        _watcher.Filter = "*.*";
    }

    async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var extention = Path.GetExtension(e.FullPath);
        if(extention == ".txt")
        {
            var handler = new TextFileHandler(e.FullPath);
            var transactions = await handler.ExecuteAsync();
            var output = FileProcessor.Transform(transactions);
            return;
        }
    }


}
