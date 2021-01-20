using DutyCycle.Triggers;
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
                builder.HasMany<GroupActionTrigger>().WithOne();
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

            modelBuilder.Entity<GroupActionTrigger>(builder =>
            {
                builder.HasKey("GroupId", "Action");
                builder.HasMany<TriggerCallback>("_callbacks").WithOne();
            });

            modelBuilder.Entity<TriggerCallback>(builder =>
            {
                builder.HasKey(callback => callback.Id);
                builder.Property(callback => callback.Id).ValueGeneratedNever();
                builder
                    .HasDiscriminator<string>("CallbackType")
                    .HasValue<SendSlackMessageCallback>("send_slack_message");
            });
        }
        
        public DbSet<Group> Groups { get; set; }
    }
}