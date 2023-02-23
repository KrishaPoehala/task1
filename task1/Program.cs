using System.Configuration;
using task1.FileProcessor;
using task1.FileWatcher;


Console.WriteLine("Welcome to the program!");
while (true)
{
    Console.WriteLine("Enter the command: ");
    var sourceStr = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(sourceStr))
    {
        continue;
    }

    await HandleCommand(sourceStr);
}

async Task HandleCommand(string sourceStr)
{
    FileWatcher fileWatcher = null;
    switch (sourceStr.Trim())
    {
        case "Start":
            var pathToListen = ConfigurationManager.AppSettings.Get("A");
            if(string.IsNullOrWhiteSpace(pathToListen) == false)
            {
                var fileProccessor = new FileProccessor();
                await fileProccessor.ProccessExistingFiles(pathToListen);
                fileWatcher = new FileWatcher(pathToListen, fileProccessor);
                Console.WriteLine("The service has been activated");
            }
        break;
        case "Stop":
            fileWatcher?.Dispose();
        break;
    }
}

