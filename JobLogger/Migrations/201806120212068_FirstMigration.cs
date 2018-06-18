namespace EntityFramework.CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        //public override void Down()
        //{
        //    DropTable("dbo.LogValues");
        //}
    }
}
