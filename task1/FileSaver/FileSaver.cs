using Newtonsoft.Json;
using System.Configuration;
using System.Text.RegularExpressions;
using task1.Helpers;

namespace task1.FileSaver;

public class FileSaver
{
    private int _fileNumber;
    private string _currentFolderPath;
    private string _currentFolderName;
    private readonly System.Timers.Timer _timer;
    private readonly ICollection<Entities.FileInfo> _data;
    public FileSaver()
    {
        var basicPath = ConfigurationManager.AppSettings.Get("B");
        _currentFolderName = DateTime.Today.ToString("MM/dd/yyyy");
        _currentFolderPath = Path.Combine(basicPath, _currentFolderName);
        if (!Directory.Exists(_currentFolderPath))
        {
            Directory.CreateDirectory(_currentFolderPath);
        }
        _timer = new();
        _timer.Interval = GetTimerInterval();
        _timer.Elapsed += Elapsed;
        _timer.Enabled = true;
        _data = new List<Entities.FileInfo>();
        _fileNumber = GetCurrentFileNumber();
    }

    private int GetCurrentFileNumber()
    {
        var info = new DirectoryInfo(_currentFolderPath);
        var lastFileName = info.GetFiles("*.json").Last();
        if(lastFileName is null)
        {
            return 1;
        }

        string lastFileNumber = new(lastFileName.Name.Skip(6).TakeWhile(x => x != '.').ToArray());
        return int.Parse(lastFileNumber) + 1;
    }

    private async void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        await ElapsedAsync();
    }

    private async Task ElapsedAsync()
    {
        var parsedFiles = _data.Count;
        var parsedLines = _data.Sum(x => x.Transactions.Count + x.InvalidLinesCount);
        var foundErrors = _data.Sum(x => x.InvalidLinesCount);
        var invalidFiles = _data.Select(x => x.FullPath).ToArray();
        var str = JsonConvert.SerializeObject(new
        {
            parsedFiles,
            parsedLines,
            foundErrors,
            invalidFiles
        });

        var logPath = Path.Combine(_currentFolderPath + "/meta.log");
        await File.WriteAllTextAsync(logPath, str);
        Reset();
    }

    private void Reset()
    {
        _currentFolderName = DateTime.Today.AddHours(23).ToString("MM/dd/yyyy");
        _fileNumber = 1;
    }

    public async Task RememberAsync(Entities.FileInfo fileInfo)
    {
        var fileName = Path.Combine(_currentFolderPath 
            + $"/output{_fileNumber}.json");
        var output = DataTransformer.Transform(fileInfo.Transactions);
        var str = JsonConvert.SerializeObject(output);
        await File.WriteAllTextAsync(fileName, str);
        _data.Add(fileInfo);
        ++_fileNumber;
    }

    private static double GetTimerInterval() => (DateTime.Today
            .AddHours(23).AddMinutes(59).AddSeconds(59) - DateTime.Now)
            .TotalMilliseconds;
}
