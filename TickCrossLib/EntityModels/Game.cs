namespace TickCrossLib.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Game")]
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            GameHistory = new HashSet<GameHistory>();
            TempGame = new HashSet<TempGame>();
        }

        public int Id { get; set; }

        public int? FirstPlayerId { get; set; }

        public int? SecondPlayerId { get; set; }

        public int? WinnerId { get; set; }

        public bool? IsDraw { get; set; }

        public int? FirstSignId { get; set; }

        public int? SecondSignId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public virtual User User { get; set; }

        public virtual SignType SignType { get; set; }

        public virtual User User1 { get; set; }

        public virtual SignType SignType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameHistory> GameHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempGame> TempGame { get; set; }
    }
}
