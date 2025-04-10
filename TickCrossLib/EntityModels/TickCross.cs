using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TickCrossLib.EntityModels
{
    public partial class TickCross : DbContext
    {
        public TickCross()
            : base("name=TickCross")
        {
        }

        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameHistory> GameHistory { get; set; }
        public virtual DbSet<SignType> SignType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFriend> UserFriend { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SignType>()
                .Property(e => e.Type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.Game)
                .WithOptional(e => e.SignType)
                .HasForeignKey(e => e.FirstSignId);

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.Game1)
                .WithOptional(e => e.SignType1)
                .HasForeignKey(e => e.SecondSignId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Game)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FirstPlayerId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Game1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.SecondPlayerId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.GameHistory)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.StepperId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserFriend)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FriendId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserFriend1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UserId);
        }
    }
}
