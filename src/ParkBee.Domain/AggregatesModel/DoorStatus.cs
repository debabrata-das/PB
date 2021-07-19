namespace ParkBee.Domain.AggregatesModel
{
    public enum DoorStatus  
    {
        Offline,
        Online,
        OnlineButFull,
        OnlineAndAlmostFull,
        OnlineAndQuiteEmpty
    }
}