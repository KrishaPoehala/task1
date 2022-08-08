using Newtonsoft.Json;
using System.Configuration;
using System.Text.Json.Serialization;
using task1.FileHandlers;
using task1.Helpers;

namespace task1.FileWatcher;

public class FileWatcher : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly FileSaver.FileSaver _fileSaver;
    public FileWatcher(string pathToListen)
    {
        _fileSaver = new();
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
        var extention = Path.GetExtension(e.FullPath);
        Entities.FileInfo fileInfo;
        if (extention == ".txt")
        {
            var fileHandler = new TextFileHandler(e.FullPath);
            fileInfo = await fileHandler.ExecuteAsync();
        }
        else if (extention == ".csv")
        {
            var csvHandler = new CsvFileHandler(e.FullPath);
            fileInfo = await csvHandler.ExecuteAsync();
        }
        else return;

        await _fileSaver.Remember(fileInfo);
    }


}
