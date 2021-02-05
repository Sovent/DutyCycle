using Cronos;
using DutyCycle.Triggers;
using DutyCycle.Common;
using DutyCycle.Organizations;
using DutyCycle.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutyCycle.Infrastructure.EntityFramework
{
    public class DutyCycleDbContext : IdentityDbContext<User, Role, int>
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

            modelBuilder.Entity<Organization>(builder =>
            {
                builder.HasKey(organization => organization.Id);
                builder.HasMany<Group>().WithOne().HasForeignKey(group => group.OrganizationId).IsRequired();
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasOne<Organization>().WithMany().HasForeignKey(user => user.OrganizationId).IsRequired();
            });
        }
        
        public DbSet<Group> Groups { get; set; }
        
        public DbSet<Organization> Organizations { get; set; }
    }
}