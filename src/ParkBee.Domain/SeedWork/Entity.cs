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
            if (IsTransient())
            {
                return default(Guid).GetHashCode();
            }

            if (_requestedHashCode.HasValue)
            {
                return _requestedHashCode.Value;
            }

            _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
            return _requestedHashCode.Value;
        }
    }
}
