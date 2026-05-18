package com.sample.inventory;

import com.sample.inventory.model.Product;
import com.sample.inventory.service.InventoryService;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;

public class InventoryApp {
    public static void main(String[] args) {
        System.out.println("=== Inventory Service Demo ===\n");

        InventoryService service = new InventoryService();

        // Add products
        service.addProduct(new Product("P001", "Mechanical Keyboard", "electronics", new BigDecimal("89.99"), 50));
        service.addProduct(new Product("P002", "USB-C Hub", "electronics", new BigDecimal("34.99"), 120));
        service.addProduct(new Product("P003", "Standing Desk Mat", "furniture", new BigDecimal("49.99"), 8));
        service.addProduct(new Product("P004", "Monitor Light Bar", "electronics", new BigDecimal("59.99"), 30));
        service.addProduct(new Product("P005", "Desk Organizer", "furniture", new BigDecimal("24.99"), 5));
        service.addProduct(new Product("P006", "Webcam HD", "electronics", new BigDecimal("79.99"), 15));

        // List all products
        System.out.println("All products:");
        for (Product p : service.getAllProducts()) {
            System.out.println("  " + p);
        }

        // Stock operations
        service.shipStock("P001", 20, "Order #1001");
        service.shipStock("P002", 50, "Order #1002");
        service.receiveStock("P003", 25, "Restock from supplier");
        service.adjustStock("P005", -2, "Damaged items");

        System.out.println("\nAfter stock movements:");
        for (Product p : service.getAllProducts()) {
            System.out.printf("  %s: %d units%s%n", p.getName(), p.getQuantity(),
                    p.needsReorder() ? " [LOW STOCK]" : "");
        }

        // Low stock alert
        List<Product> lowStock = service.getLowStockProducts();
        System.out.println("\nLow stock alerts:");
        for (Product p : lowStock) {
            System.out.printf("  ⚠ %s: %d units (reorder at %d)%n",
                    p.getName(), p.getQuantity(), p.getReorderLevel());
        }

        // Summary
        System.out.println("\nStock summary by category:");
        Map<String, Integer> summary = service.getStockSummaryByCategory();
        for (Map.Entry<String, Integer> entry : summary.entrySet()) {
            System.out.printf("  %s: %d total units%n", entry.getKey(), entry.getValue());
        }

        System.out.printf("\nTotal inventory value: $%s%n", service.getTotalInventoryValue());

        // Movement history
        System.out.println("\nMovement history for P001:");
        service.getMovementsForProduct("P001").forEach(m ->
                System.out.println("  " + m));
    }
}
