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

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                _requestedHashCode ??= Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
             
            return base.GetHashCode();
        }

        /*public static bool operator ==(Entity left, Entity right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }*/
    }
}
