using System;
using System.Data.Entity;
using DataModel.Enities;

namespace DataAccess
{
    public class SjInitializer : DropCreateDatabaseIfModelChanges<SjContext>
    {
        protected override void Seed(SjContext context)
        {
            var firstUser = new User { Name = "user1" };
            var secondUser = new User { Name = "user2" };

            context.Messages.AddRange(new[]
            {
                new Message
                {
                    Text = "firstMessageFromFirstUser",
                    User = firstUser,
                    CreationDateTime = DateTime.Now
                },
                new Message
                {
                    Text = "secondMessageFromFirstUser",
                    User = firstUser,
                    CreationDateTime = DateTime.Now.AddMinutes(2)
                },
                new Message
                {
                    Text = "firstMessageFromSecondUser",
                    User = secondUser,
                    CreationDateTime = DateTime.Now.AddMinutes(1)
                },
                new Message
                {
                    Text = "secondMessageFromSecondUser",
                    User = secondUser,
                    CreationDateTime = DateTime.Now.AddMinutes(3)
                }
            });

            context.SaveChanges();
        }
    }
}