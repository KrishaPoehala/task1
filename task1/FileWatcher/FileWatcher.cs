using Newtonsoft.Json;
using System.Configuration;
using System.Text.Json.Serialization;
using task1.FileHandlers;
using task1.Helpers;

namespace task1.FileWatcher;

public class FileWatcher
{
    private readonly FileSystemWatcher _watcher;
    private readonly FileSaver.FileSaver _fileSaver;
    public FileWatcher()
    {
        _fileSaver = new();
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
        _watcher.Filter = "*.*";//prop filter does not support multipule file filters
    }

    async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var extention = Path.GetExtension(e.FullPath);
        if(extention == ".txt")
        {
            var handler = new TextFileHandler(e.FullPath);
            var fileInfo = await handler.ExecuteAsync();
            await _fileSaver.Remember(fileInfo);
            return;
        }
    }


}
