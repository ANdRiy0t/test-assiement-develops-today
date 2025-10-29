namespace Test.AssiementDevelopsToday.Jobs.Entities;

// Created this model by SRP (first principal SOLID) 
public class DuplicateRecord
{
    public DateTime PickupDateTimeUtc { get; set; }
    public DateTime DropoffDateTimeUtc { get; set; }
    public int PassengerCount { get; set; }
    public decimal TripDistance { get; set; }
    public string StoreAndFwdFlag { get; set; } = "";
    public int PULocationID { get; set; }
    public int DOLocationID { get; set; }
    public decimal FareAmount { get; set; }
    public decimal TipAmount { get; set; }
}