namespace TickCrossLib.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserFriend")]
    public partial class UserFriend
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? FriendId { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
