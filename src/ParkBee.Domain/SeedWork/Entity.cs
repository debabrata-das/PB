using System;
namespace ParkBee.Domain.SeedWork
{
    public abstract class Entity
    {
        int? _requestedHashCode;
        public virtual Guid Id { get; protected set; }

        public bool IsTransient()
        {
            return Id == default(Guid);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Entity;
            if (item is null)
            {
                return false;
            }

            if (ReferenceEquals(this, item))
            {
                return true;
            }

            if (GetType() != item.GetType())
            {
                return false;
            }

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }
            return item.Id == Id;
        }
    }
}
