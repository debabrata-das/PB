using System;
using ParkBee.Domain.Exceptions;

namespace ParkBee.WebApplication.Server.Exceptions
{
    [Serializable]
    public class CannotAddDuplicateEntityException : ParkBeeBaseException
    {
        public string EntityName { get; set; }
        public Guid GuidIdentifier { get; set; }
        public int IntIdentifier { get; set; }

        public CannotAddDuplicateEntityException(string entityName, Guid identifier)
            : base($"Cannot add duplicate {entityName} with {identifier}")
        {
            EntityName = entityName;
            GuidIdentifier = identifier;
        }

        public CannotAddDuplicateEntityException(string entityName, int identifier)
            : base($"Cannot add duplicate {entityName} with {identifier}")
        {
            EntityName = entityName;
            IntIdentifier = identifier;
        }
    }
}