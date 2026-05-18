package com.sample.inventory.model;

import java.time.LocalDateTime;

public class StockMovement {
    public enum MovementType {
        RECEIPT, SHIPMENT, ADJUSTMENT, RETURN
    }

    private String id;
    private String productId;
    private MovementType type;
    private int quantity;
    private String reason;
    private LocalDateTime timestamp;

    public StockMovement(String id, String productId, MovementType type, int quantity, String reason) {
        this.id = id;
        this.productId = productId;
        this.type = type;
        this.quantity = quantity;
        this.reason = reason;
        this.timestamp = LocalDateTime.now();
    }

    public String getId() { return id; }
    public String getProductId() { return productId; }
    public MovementType getType() { return type; }
    public int getQuantity() { return quantity; }
    public String getReason() { return reason; }
    public LocalDateTime getTimestamp() { return timestamp; }

    public int getEffectiveQuantity() {
        switch (type) {
            case RECEIPT:
            case RETURN:
                return quantity;
            case SHIPMENT:
                return -quantity;
            case ADJUSTMENT:
                return quantity; // can be negative
            default:
                return 0;
        }
    }

    @Override
    public String toString() {
        return String.format("StockMovement{type=%s, productId='%s', qty=%d, reason='%s'}",
                type, productId, quantity, reason);
    }
}
