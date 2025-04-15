using FluentMigrator;

namespace Persistence.Migrations
{
    [Migration(2024041501, "Add indexing for performance matter in order products")]
    public class AddOrderProductIndexes : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE INDEX IF NOT EXISTS idx_order_products_order_id
                ON order_products(order_id);

                CREATE INDEX IF NOT EXISTS idx_order_products_product_id
                ON order_products(product_id);

                CREATE INDEX IF NOT EXISTS idx_products_discount_percentage
                ON products(discount_percentage);
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                DROP INDEX IF EXISTS idx_products_discount_percentage;
                DROP INDEX IF EXISTS idx_order_products_product_id;
                DROP INDEX IF EXISTS idx_order_products_order_id;
            ");
        }
    }
}