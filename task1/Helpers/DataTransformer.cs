using task1.Entities;

namespace task1.Helpers;

public static class DataTransformer
{
    public static ICollection<OutputType> Transform(ICollection<Transaction> transactions)
    {
        var output = new List<OutputType>();
        var grouped = transactions.GroupBy(x => x.City);
        foreach (var item in grouped)
        {
            var o = TransformSingle(item);
            output.Add(o);
        }

        return output;
    }  
    
    private static OutputType TransformSingle(IGrouping<string, Transaction> grouped)
    {
        var o = new OutputType();
        o.City = grouped.Key;
        o.Total = grouped.Sum(x => x.Payment);

        var servicesGrouped = grouped.GroupBy(x => x.Service);
        foreach (var itemService in servicesGrouped)
        {
            var service = TransformService(itemService, o);
        }

        return o;
    }

    private static Service TransformService(IGrouping<string, Transaction> serviceItem,
        OutputType o)
    {
        var service = new Service();
        service.Name = serviceItem.Key;
        service.Total = serviceItem.Sum(x => x.Payment);
        var payersGrouped = serviceItem.GroupBy(x => x.FirstName).ToList();
        foreach (var item in payersGrouped)
        {
            TransformPayer(item, service);
        }

        o.Services.Add(service);
        return service;
    }

    private static void TransformPayer(IGrouping<string, Transaction> grouped,
        Service service)
    {
        foreach (var transaction in grouped)
        {
            var p = new Payer(transaction);
            service.Payers.Add(p);
        }
    }
}
