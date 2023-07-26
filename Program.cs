using EFCore_DBLibrary;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore_Activity0302;

public class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<InventoryDBContext>
    _optionsBuilder;
    static void Main(string[] args)
    {
        BuildOptions();
        EnsureItems();
        ListInventory();
    }
    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDBContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString(
       "InventoryManager"));
    }

    static void EnsureItems()
    {
        EnsureItem("Batman Begins");
        EnsureItem("Inception");
        EnsureItem("Star Wars");
        EnsureItem("Sonds of Freedom");
        EnsureItem("Top Gun");
    }

    private static void EnsureItem(string name)
    {
        using (var db = new InventoryDBContext(_optionsBuilder.Options))
        {
            var existingItems = db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (existingItems == null)
            {
                var item = new Item() { Name = name };
                db.Items.Add(item);
                db.SaveChanges();
            }
        }
    }

    private static void ListInventory()
    {
        using (var db = new InventoryDBContext(_optionsBuilder.Options))
        {
            var items = db.Items.OrderBy(x => x.Name).ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x.Name}"));
        }
    }
}
