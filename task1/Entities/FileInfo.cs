namespace task1.Entities;

public class FileInfo
{
    public ICollection<Transaction> Transactions { get; set; }
    public int InvalidLinesCount { get; set; }
    public string FullPath { get; set; }
}
