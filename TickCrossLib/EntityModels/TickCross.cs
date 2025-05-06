using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TickCrossLib.EntityModels
{
    public partial class TickCross : DbContext
    {
        public TickCross()
            : base(@"data source=(localdb)\MSSQLLocalDB;initial catalog=TickCrossClientServer;integrated security=True;MultipleActiveResultSets=True")
        {
        }

        public virtual DbSet<FriendOffer> FriendOffer { get; set; }
        public virtual DbSet<FriendReqStatus> FriendReqStatus { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameHistory> GameHistory { get; set; }
        public virtual DbSet<GameRequest> GameRequest { get; set; }
        public virtual DbSet<RequestStatus> RequestStatus { get; set; }
        public virtual DbSet<SignType> SignType { get; set; }
        public virtual DbSet<TempGame> TempGame { get; set; }
        public virtual DbSet<TempGameType> TempGameType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFriend> UserFriend { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendReqStatus>()
                .HasMany(e => e.FriendOffer)
                .WithOptional(e => e.FriendReqStatus)
                .HasForeignKey(e => e.StatusId);

            modelBuilder.Entity<RequestStatus>()
                .HasMany(e => e.GameRequest)
                .WithOptional(e => e.RequestStatus)
                .HasForeignKey(e => e.StatusId);

            modelBuilder.Entity<SignType>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.Game)
                .WithOptional(e => e.SignType)
                .HasForeignKey(e => e.FirstSignId);

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.Game1)
                .WithOptional(e => e.SignType1)
                .HasForeignKey(e => e.SecondSignId);

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.GameRequest)
                .WithOptional(e => e.SignType)
                .HasForeignKey(e => e.ReciverSignId);

            modelBuilder.Entity<SignType>()
                .HasMany(e => e.GameRequest1)
                .WithOptional(e => e.SignType1)
                .HasForeignKey(e => e.SenderSignId);

            modelBuilder.Entity<TempGameType>()
                .HasMany(e => e.TempGame)
                .WithOptional(e => e.TempGameType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FriendOffer)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.ReciverId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FriendOffer1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.SenderId);

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
                .HasMany(e => e.GameRequest)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.ReciverId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.GameRequest1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.SenderId);

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
