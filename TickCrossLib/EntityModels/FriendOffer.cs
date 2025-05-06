namespace TickCrossLib.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FriendOffer")]
    public partial class FriendOffer
    {
        public int Id { get; set; }

        public int? SenderId { get; set; }

        public int? ReciverId { get; set; }

        public int? StatusId { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual FriendReqStatus FriendReqStatus { get; set; }
    }
}
