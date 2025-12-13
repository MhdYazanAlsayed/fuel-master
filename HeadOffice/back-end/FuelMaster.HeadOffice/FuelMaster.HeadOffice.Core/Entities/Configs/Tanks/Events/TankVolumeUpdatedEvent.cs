namespace FuelMaster.HeadOffice.Core.Entities.Configs.Tanks.Events
{
    /// <summary>
    /// Example domain event that is raised when a tank's volume is updated.
    /// This event implements IDomainEvent which extends INotification,
    /// so MediatR will automatically route it to TankVolumeUpdatedHandler.
    /// </summary>
    public class TankVolumeUpdatedEvent : DomainEventBase
    {
        public int TankId { get; }
        public decimal OldVolume { get; }
        public decimal NewVolume { get; }

        public TankVolumeUpdatedEvent(int tankId, decimal oldVolume, decimal newVolume)
        {
            TankId = tankId;
            OldVolume = oldVolume;
            NewVolume = newVolume;
        }
    }
}

