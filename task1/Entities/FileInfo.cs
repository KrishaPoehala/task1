namespace task1.Entities;

public class FileInfo
{
    public ICollection<Transaction> Transactions { get; set; }
    public int InvalidLinesCount { get; set; }
    public bool IsFileInvalid => InvalidLinesCount > 0;
    public string FullPath { get; set; }
}
