namespace TickCrossLib.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TempGame")]
    public partial class TempGame
    {
        public int Id { get; set; }

        public int? TempMoveX { get; set; }

        public int? TempMoveY { get; set; }

        public int? TypeId { get; set; }

        public int? GameId { get; set; }

        public int? StepperId { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }

        public virtual TempGameType TempGameType { get; set; }
    }
}
