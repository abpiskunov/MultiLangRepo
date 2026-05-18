package com.sample.inventory.service;

import com.sample.inventory.model.Product;
import com.sample.inventory.model.StockMovement;

import java.math.BigDecimal;
import java.util.*;
import java.util.stream.Collectors;

public class InventoryService {
    private final Map<String, Product> products = new LinkedHashMap<>();
    private final List<StockMovement> movements = new ArrayList<>();

    public void addProduct(Product product) {
        if (products.containsKey(product.getId())) {
            throw new IllegalArgumentException("Product already exists: " + product.getId());
        }
        products.put(product.getId(), product);
    }

    public Optional<Product> getProduct(String id) {
        return Optional.ofNullable(products.get(id));
    }

    public List<Product> getAllProducts() {
        return new ArrayList<>(products.values());
    }

    public List<Product> getProductsByCategory(String category) {
        return products.values().stream()
                .filter(p -> p.getCategory().equalsIgnoreCase(category))
                .collect(Collectors.toList());
    }

    public List<Product> getLowStockProducts() {
        return products.values().stream()
                .filter(Product::needsReorder)
                .collect(Collectors.toList());
    }

    public void receiveStock(String productId, int quantity, String reason) {
        Product product = getProductOrThrow(productId);
        StockMovement movement = new StockMovement(
                UUID.randomUUID().toString(), productId,
                StockMovement.MovementType.RECEIPT, quantity, reason);
        movements.add(movement);
        product.setQuantity(product.getQuantity() + quantity);
    }

    public void shipStock(String productId, int quantity, String reason) {
        Product product = getProductOrThrow(productId);
        if (product.getQuantity() < quantity) {
            throw new IllegalStateException(
                    String.format("Insufficient stock for %s: have %d, need %d",
                            productId, product.getQuantity(), quantity));
        }
        StockMovement movement = new StockMovement(
                UUID.randomUUID().toString(), productId,
                StockMovement.MovementType.SHIPMENT, quantity, reason);
        movements.add(movement);
        product.setQuantity(product.getQuantity() - quantity);
    }

    public void adjustStock(String productId, int quantity, String reason) {
        Product product = getProductOrThrow(productId);
        StockMovement movement = new StockMovement(
                UUID.randomUUID().toString(), productId,
                StockMovement.MovementType.ADJUSTMENT, quantity, reason);
        movements.add(movement);
        product.setQuantity(product.getQuantity() + quantity);
    }

    public BigDecimal getTotalInventoryValue() {
        return products.values().stream()
                .map(Product::getTotalValue)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }

    public List<StockMovement> getMovementsForProduct(String productId) {
        return movements.stream()
                .filter(m -> m.getProductId().equals(productId))
                .collect(Collectors.toList());
    }

    public Map<String, Integer> getStockSummaryByCategory() {
        return products.values().stream()
                .collect(Collectors.groupingBy(
                        Product::getCategory,
                        Collectors.summingInt(Product::getQuantity)));
    }

    private Product getProductOrThrow(String productId) {
        return getProduct(productId).orElseThrow(
                () -> new NoSuchElementException("Product not found: " + productId));
    }
}
