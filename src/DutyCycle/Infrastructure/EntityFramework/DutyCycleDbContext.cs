using Cronos;
using DutyCycle.Triggers;
using DutyCycle.Common;
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
                builder
                    .Property(group => group.CyclingCronExpression)
                    .HasConversion(
                        cron => cron.ToString(CronFormat.Standard),
                        expressionString => CronExpression.Parse(expressionString));
                builder.Ignore(group => group.Members);
                builder.Ignore(group => group.Info);
                builder.HasMany<RotationChangedTrigger>("_triggers").WithOne();
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

            modelBuilder.Entity<RotationChangedTrigger>(builder =>
            {
                builder.HasKey(trigger => trigger.Id);
                builder.Property(trigger => trigger.Id).ValueGeneratedNever();
                builder
                    .HasDiscriminator<string>("TriggerType")
                    .HasValue<SendSlackMessageTrigger>("send_slack_message");
            });
        }
        
        public DbSet<Group> Groups { get; set; }
    }
}