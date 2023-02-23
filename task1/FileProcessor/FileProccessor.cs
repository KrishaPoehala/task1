using task1.FileHandlers;

namespace task1.FileProcessor;

public class FileProccessor:IFileProccessor
{
    private BaseFileHandler fileHandler;
    private readonly FileSaver.FileSaver _fileSaver;

    public FileProccessor()
    {
        _fileSaver = new();
    }

    public async Task ProccessFile(string filePath)
    {
        var extention = Path.GetExtension(filePath);
        fileHandler = extention switch
        {
            ".txt" => new TextFileHandler(filePath),
            ".csv" => new CsvFileHandler(filePath),
            _ => null,
        };

        if(fileHandler is null)
        {
            return;
        }

        Entities.FileInfo fileInfo = await fileHandler.ExecuteAsync();
        await _fileSaver.RememberAsync(fileInfo);
    }

    public async Task ProccessExistingFiles(string initialPath)
    {
        var files = Directory.GetFiles(initialPath);
        foreach (var file in files)
        {
            await ProccessFile(file);
        }
    }
}
