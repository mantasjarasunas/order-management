using FluentMigrator;

namespace Persistence.Migrations
{
    [Migration(2024041402, "Extend products with discount and create orders structure with quantity per product")]
    public class CreateOrderingStructure : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                ALTER TABLE products
                ADD COLUMN discount_percentage NUMERIC(5,2),
                ADD COLUMN discount_quantity_threshold INT,
                ADD CONSTRAINT chk_discount_percentage CHECK (
                    discount_percentage IS NULL OR (discount_percentage >= 0 AND discount_percentage <= 100)
                ),
                ADD CONSTRAINT chk_discount_quantity CHECK (
                    discount_quantity_threshold IS NULL OR discount_quantity_threshold >= 0
                );

                UPDATE products
                SET 
                    discount_percentage = 0,
                    discount_quantity_threshold = 0
                WHERE 
                    discount_percentage IS NULL
                    AND discount_quantity_threshold IS NULL;

                CREATE TABLE orders (
                    id BIGSERIAL PRIMARY KEY,
                    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
                );

                CREATE TABLE order_products (
                    id BIGSERIAL PRIMARY KEY,
                    order_id BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
                    product_id BIGINT NOT NULL REFERENCES products(id),
                    quantity INT NOT NULL CHECK (quantity > 0)
                );
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                DROP TABLE IF EXISTS order_products;
                DROP TABLE IF EXISTS orders;

                ALTER TABLE products
                DROP CONSTRAINT IF EXISTS chk_discount_percentage,
                DROP CONSTRAINT IF EXISTS chk_discount_quantity,
                DROP COLUMN IF EXISTS discount_percentage,
                DROP COLUMN IF EXISTS discount_quantity_threshold;
            ");
        }
    }
}