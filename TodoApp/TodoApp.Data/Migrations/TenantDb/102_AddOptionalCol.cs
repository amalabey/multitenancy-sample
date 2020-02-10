using FluentMigrator;

namespace TodoApp.Data.Migrations.TenantDb
{
    [Migration(102)]
    public class AddOptionalCol : Migration
    {
        public override void Up()
        {
            Alter.Table("TodoItems")
                .AddColumn("Assignee").AsString().Nullable();
        }

        public override void Down()
        {
        }
    }
}
