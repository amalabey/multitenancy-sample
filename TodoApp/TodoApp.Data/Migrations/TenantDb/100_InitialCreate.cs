using FluentMigrator;

namespace TodoApp.Data.Migrations.TenantDb
{
    [Migration(100)]
    public class AddLogTable : Migration
    {
        public override void Up()
        {
            Create.Table("TodoItems")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Description").AsString()
                .WithColumn("IsComplete").AsBoolean();
        }

        public override void Down()
        {
            Delete.Table("TodoItems");
        }
    }
}
