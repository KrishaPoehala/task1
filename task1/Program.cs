
using System.Configuration;
using System.Globalization;
using task1.FileWatcher;
using task1.Helpers;


Console.WriteLine("Welcome to the program!");
while (true)
{
    Console.WriteLine("Enter the command: ");
    var sourceStr = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(sourceStr))
    {
        continue;
    }

    HandlerCommand(sourceStr);
}

void HandlerCommand(string sourceStr)
{
    FileWatcher fileWatcher = null;
    switch (sourceStr.Trim())
    {
        case "Start":
            var pathToListen = ConfigurationManager.AppSettings.Get("A");
            if(string.IsNullOrWhiteSpace(pathToListen) == false)
            {
                Console.WriteLine("The service has been activated");
                fileWatcher = new FileWatcher(pathToListen);
            }
        break;
        case "Stop":
            fileWatcher?.Dispose();
        break;
    }
}

