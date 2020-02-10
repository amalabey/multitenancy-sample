using FluentMigrator;

namespace TodoApp.Data.Migrations.TenantDb
{
    [Migration(103)]
    public class AddRequiredCol : Migration
    {
        public override void Up()
        {
            Alter.Table("TodoItems")
                .AddColumn("StoryId").AsInt32().Nullable();

            Update.Table("TodoItems")
                .Set(new { StoryId = 0 })
                .AllRows();

            Alter.Table("TodoItems")
                .AlterColumn("StoryId").AsInt32().NotNullable();
        }

        public override void Down()
        {
        }
    }
}
