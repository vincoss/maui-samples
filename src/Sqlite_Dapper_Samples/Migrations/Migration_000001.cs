using FluentMigrator;
using Sqlite_Dapper_Samples.Entities;
using System;

namespace Sqlite_Dapper_Samples.Migrations
{
    // TODO: Collation("NOCASE")

    [Migration(000001)]
    public class Migration_000001 : Migration
    {
        public override void Up()
        {
            Item_Up();
        }

        public override void Down()
        {
            Delete.Table(nameof(Item));
        }

        private void Item_Up()
        {
            Create.Table(nameof(Item))
                .WithColumn(nameof(Item.ItemId)).AsInt32().PrimaryKey().Identity().NotNullable().Indexed()
                .WithColumn(nameof(Item.JobId)).AsInt32().NotNullable().Indexed()
                .WithColumn(nameof(Item.Name)).AsString(64).NotNullable().Unique()
                .WithColumn(nameof(Item.Description)).AsString(512)

                .WithColumn(nameof(Item.CreatedDateUtc)).AsDateTime2().NotNullable().WithDefaultValue(DateTime.UtcNow)
                .WithColumn(nameof(Item.DueDateUtc)).AsDateTime2()
                .WithColumn(nameof(Item.Status)).AsString(32)
                .WithColumn(nameof(Item.Priority)).AsString(32)
                .WithColumn(nameof(Item.Assigne)).AsString(512)
                .WithColumn(nameof(Item.Tags)).AsString(512)

                .WithColumn(nameof(Item.CreatedDateTimeUtc)).AsDateTime2().NotNullable().WithDefaultValue(DateTime.UtcNow)
                .WithColumn(nameof(Item.ModifiedDateTimeUtc)).AsDateTime2().NotNullable().WithDefaultValue(DateTime.UtcNow)
                .WithColumn(nameof(Item.RowUuid)).AsGuid().NotNullable();
        }
    }
}