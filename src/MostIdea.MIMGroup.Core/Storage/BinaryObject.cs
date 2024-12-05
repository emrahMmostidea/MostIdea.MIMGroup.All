using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;

namespace MostIdea.MIMGroup.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual string Description { get; set; }

        [Required] 
        public virtual byte[] Bytes { get; set; }

        public virtual Guid RowId { get; set; }

        public virtual string TableName { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();

        }

        public BinaryObject(int? tenantId, byte[] bytes, string description = null, string tableName = null, Guid rowId = new Guid())
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            TableName = tableName;
            rowId = RowId;
        }
    }
}