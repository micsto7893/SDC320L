/****************************************************************************
Name: Michael Stoller
Date: 11/15/2026
Assignment: Inheritance, Composition, and User Interactions
****************************************************************************/

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Sample equipment demonstrates realistic records without requiring a database.
        var well = new WellPump("EQ-001", "Main House Well Pump", "In Service", 7.0);
        well.AddMaintenance(new MaintenanceRecord("2026-10-15", "Inspected pressure system", "Routine check"));

        // A generator is being considered; it is NOT marked as installed equipment.
        var generator = new Generator("EQ-002", "Whole-Home Standby Generator", "Planned", "Propane");
        generator.AddMaintenance(new MaintenanceRecord("Not scheduled", "Obtain installation quotes", "Planning task"));

        List<Equipment> equipment = new List<Equipment> { well, generator };

        WriteCentered("============================================");
        WriteCentered("DEER RUN PROPERTY MANAGER");
        WriteCentered("(project week 1)");
        WriteCentered("Created By: Michael Stoller");
        WriteCentered("============================================");
        WriteCentered("\nThis program keeps property equipment and service notes organized.");
        WriteCentered("Enter a menu number and press Enter to make a selection.");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("1. View equipment list");
            Console.WriteLine("2. View equipment details and maintenance notes");
            Console.WriteLine("0. Exit");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nID       Equipment                         Status");
                    Console.WriteLine("-----------------------------------------------------------");
                    foreach (Equipment item in equipment)
                        Console.WriteLine($"{item.Id,-8} {item.Name,-33} {item.Status}");
                    break;
                case "2":
                    Console.Write("Enter equipment ID (for example, EQ-001): ");
                    string? id = Console.ReadLine();
                    Equipment? selected = equipment.Find(item =>
                        item.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                    if (selected == null)
                    {
                        Console.WriteLine("No equipment found with that ID.");
                        break;
                    }
                    selected.DisplayDetails();
                    break;
                case "0":
                    Console.WriteLine("All done for today? Ok...goodbye!");
                    running = false;
                    break;
                                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 0.");
                    break;
            }
        }
    }

// Centers text within a 60-character-wide console layout.
static void WriteCentered(string text)
{
    int layoutWidth = 60;
    int leftPadding = Math.Max((layoutWidth - text.Length) / 2, 0);
    Console.WriteLine(new string(' ', leftPadding) + text);
}
}

// Shared properties for all property equipment.
class Equipment
{
    public string Id { get; }
    public string Name { get; }
    public string Status { get; }

    // Equipment has a collection of MaintenanceRecord objects.
    private readonly List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();

    public Equipment(string id, string name, string status)
    {
        Id = id;
        Name = name;
        Status = status;
    }

    public void AddMaintenance(MaintenanceRecord record) => maintenanceRecords.Add(record);

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"\n{Id} - {Name} ({Status})");
        Console.WriteLine("Maintenance / planning notes:");
        if (maintenanceRecords.Count == 0)
            Console.WriteLine("  No notes recorded.");
        foreach (MaintenanceRecord record in maintenanceRecords)
            Console.WriteLine($"  {record.Date}: {record.Task} - {record.Notes}");
    }
}

// Equipment type: well pump
class WellPump : Equipment
{
    public double WellFlowGallonsPerMinute { get; }

    public WellPump(string id, string name, string status, double flowRate)
        : base(id, name, status)
    {
        WellFlowGallonsPerMinute = flowRate;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Well flow rate: {WellFlowGallonsPerMinute:0.0} GPM");
    }
}

// Equipment type: generator
class Generator : Equipment
{
    public string FuelType { get; }

    public Generator(string id, string name, string status, string fuelType)
        : base(id, name, status)
    {
        FuelType = fuelType;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Fuel type: {FuelType}");
    }
}

// Each maintenance record belongs to an equipment item's notes.
class MaintenanceRecord
{
    public string Date { get; }
    public string Task { get; }
    public string Notes { get; }

    public MaintenanceRecord(string date, string task, string notes)
    {
        Date = date;
        Task = task;
        Notes = notes;
    }
}
