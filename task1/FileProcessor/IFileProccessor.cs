namespace task1.FileProcessor;

public interface IFileProccessor
{
    public Task ProccessExistingFiles(string initialPath);
    public Task ProccessFile(string filePath);
}
