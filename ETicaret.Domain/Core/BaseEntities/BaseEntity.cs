﻿namespace ETicaret.Domain.Core.BaseEntities
{
    public abstract class BaseEntity : IUpdatetebleEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public Status Status { get; set; }
        
    }
}
