using FluentMigrator;

namespace Persistence.Migrations
{
    [Migration(2024041401, "Create Products Table")]
    public class CreateProductsTable : Migration
    {
        public override void Up()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS products (
                    id BIGSERIAL PRIMARY KEY,
                    name TEXT NOT NULL,
                    price NUMERIC(10,2) NOT NULL,
                    created_at TIMESTAMPTZ DEFAULT NOW(),
                    updated_at TIMESTAMPTZ DEFAULT NOW()
                );
            ";

            Execute.Sql(sql);
        }

        public override void Down()
        {
            Execute.Sql($"DROP TABLE IF EXISTS products;");
        }
    }
}