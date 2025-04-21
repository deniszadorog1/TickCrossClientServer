namespace TickCrossLib.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GameRequest")]
    public partial class GameRequest
    {
        public int Id { get; set; }

        public int? SenderId { get; set; }

        public int? ReciverId { get; set; }

        public int? SenderSignId { get; set; }

        public int? ReciverSignId { get; set; }

        public int? StatusId { get; set; }

        public virtual User User { get; set; }

        public virtual SignType SignType { get; set; }

        public virtual User User1 { get; set; }

        public virtual SignType SignType1 { get; set; }

        public virtual RequestStatus RequestStatus { get; set; }
    }
}
