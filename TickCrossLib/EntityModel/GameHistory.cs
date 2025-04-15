namespace TickCrossLib.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GameHistory")]
    public partial class GameHistory
    {
        public int Id { get; set; }

        public int? GameId { get; set; }

        public int? XCordId { get; set; }

        public int? YCordId { get; set; }

        public int? StepperId { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }
    }
}
