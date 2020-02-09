using FluentMigrator;

namespace TodoApp.Data.Migrations.TenantDb
{
    [Migration(101)]
    public class ChangeIdCol : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE TodoItems DROP CONSTRAINT PK_TodoItems");
            Alter.Table("TodoItems")
                .AlterColumn("Id").AsInt32().PrimaryKey();
        }

        public override void Down()
        {
        }
    }
}
