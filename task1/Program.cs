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

    await HandleCommand(sourceStr.Trim().ToLower());
}

async Task HandleCommand(string sourceStr)
{
    FileWatcher fileWatcher = null;
    switch (sourceStr)
    {
        case "start":
            var pathToListen = ConfigurationManager.AppSettings.Get("A");
            
            if(string.IsNullOrWhiteSpace(pathToListen) == false)
            {
                var fileProccessor = new FileProccessor();
                await fileProccessor.ProccessExistingFiles(pathToListen);
                fileWatcher = new FileWatcher(pathToListen, fileProccessor);
                Console.WriteLine("The service has been activated");
                return;
            }

            Console.WriteLine("Config file path is not specified");
        break;
        case "stop":
            fileWatcher?.Dispose();
        break;
        default:
            Console.WriteLine("Invalid command. try again");
        break;
    }
}

