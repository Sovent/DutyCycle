using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Infrastructure.EntityFramework
{
    public class DutyCycleDbContext : DbContext
    {
        public DutyCycleDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>(builder =>
            {
                builder.HasKey(group => group.Id);
                builder.HasMany<GroupMember>("_groupMembers").WithOne();
                builder.Ignore(group => group.Members);
                builder.Ignore(group => group.CurrentDuties);
            });

            modelBuilder.Entity<GroupMember>(builder =>
            {
                builder.HasKey(member => member.Id);
                builder.Property(member => member.Id).ValueGeneratedNever();
                builder
                    .HasOne<GroupMember>()
                    .WithOne()
                    .HasForeignKey<GroupMember>(member => member.FollowedGroupMemberId)
                    .IsRequired(false);
            });
        }
        
        public DbSet<Group> Groups { get; set; }
    }
}