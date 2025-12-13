namespace FuelMaster.HeadOffice.Core.Entities
{
    public abstract class Entity<T>: Base 
    {
        public T Id { get; set; } = default(T)!;
    }
}
