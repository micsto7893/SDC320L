/****************************************************************************
Name: Michael Stoller
Date: 11/22/2026
Assignment: SDC320L - Week 2: Interfaces and Polymorphism
Purpose: Console-based Deer Run Property Manager. Equipment and its derived
         classes model property systems; MaintenanceRecord models notes;
         IMaintenanceTrackable provides a common maintenance-summary contract.
****************************************************************************/

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var well = new WellPump("EQ-001", "Main House Well Pump", "In Service", 7.0);
        well.AddMaintenance(new MaintenanceRecord("2026-10-15", "Inspected pressure system", "Routine check"));

        // The generator is planned, not installed equipment.
        var generator = new Generator("EQ-002", "Whole-Home Standby Generator", "Planned", "Propane");
        generator.AddMaintenance(new MaintenanceRecord("Not scheduled", "Obtain installation quotes", "Planning task"));

        // Polymorphism through inheritance: one Equipment list holds different derived types.
        List<Equipment> equipment = new List<Equipment> { well, generator };

        WriteCentered("============================================");
        WriteCentered("DEER RUN PROPERTY MANAGER");
        WriteCentered("Project Week 2 - Interfaces and Polymorphism");
        WriteCentered("Created By: Michael Stoller");
        WriteCentered("============================================");
        Console.WriteLine();
        WriteCentered("Manage property equipment and maintenance notes.");
        WriteCentered("Enter a menu number and press Enter to select.");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("1. View equipment list");
            Console.WriteLine("2. View equipment details and maintenance notes");
            Console.WriteLine("3. View maintenance summary");
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
                    // Polymorphism: virtual method calls the WellPump or Generator override.
                    selected.DisplayDetails();
                    break;
                case "3":
                    Console.WriteLine("\nMAINTENANCE SUMMARY");
                    // Interface polymorphism: different equipment types are handled through
                    // the same IMaintenanceTrackable contract.
                    foreach (IMaintenanceTrackable item in equipment)
                        Console.WriteLine(item.GetMaintenanceSummary());
                    break;
                case "0":
                    Console.WriteLine("All done for today? Ok...goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 0.");
                    break;
            }
        }
    }

    static void WriteCentered(string message)
    {
        const int layoutWidth = 60;
        int leftPadding = Math.Max((layoutWidth - message.Length) / 2, 0);
        Console.WriteLine(new string(' ', leftPadding) + message);
    }
}

// INTERFACE: defines the maintenance behavior expected of trackable equipment.
interface IMaintenanceTrackable
{
    void AddMaintenance(MaintenanceRecord record);
    string GetMaintenanceSummary();
}

// Shared base class for all equipment. Implements the maintenance interface.
class Equipment : IMaintenanceTrackable
{
    public string Id { get; }
    public string Name { get; }
    public string Status { get; }

    // COMPOSITION: equipment contains a collection of maintenance records.
    private readonly List<MaintenanceRecord> maintenanceRecords = new List<MaintenanceRecord>();

    public Equipment(string id, string name, string status)
    {
        Id = id;
        Name = name;
        Status = status;
    }

    // INTERFACE IMPLEMENTATION
    public void AddMaintenance(MaintenanceRecord record) => maintenanceRecords.Add(record);

    // INTERFACE IMPLEMENTATION
    public string GetMaintenanceSummary()
    {
        return $"{Id} - {Name}: {maintenanceRecords.Count} record(s)";
    }

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

// INHERITANCE: WellPump derives from Equipment.
class WellPump : Equipment
{
    public double WellFlowGallonsPerMinute { get; }

    public WellPump(string id, string name, string status, double flowRate)
        : base(id, name, status)
    {
        WellFlowGallonsPerMinute = flowRate;
    }

    // POLYMORPHISM: specialized version of the base display method.
    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Well flow rate: {WellFlowGallonsPerMinute:0.0} GPM");
    }
}

// INHERITANCE: Generator derives from Equipment.
class Generator : Equipment
{
    public string FuelType { get; }

    public Generator(string id, string name, string status, string fuelType)
        : base(id, name, status)
    {
        FuelType = fuelType;
    }

    // POLYMORPHISM: specialized version of the base display method.
    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Fuel type: {FuelType}");
    }
}

// Individual maintenance or planning note associated with equipment.
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
