package com.sample.json;

import java.util.*;

public class JsonToolkitApp {
    public static void main(String[] args) {
        System.out.println("=== JSON Toolkit Demo ===\n");

        // Build a JSON object
        Map<String, Object> address = new LinkedHashMap<>();
        address.put("street", "123 Main St");
        address.put("city", "Springfield");
        address.put("state", "IL");
        address.put("zip", "62701");

        JsonObject user = new JsonObject()
                .put("name", "John Doe")
                .put("email", "john@example.com")
                .put("age", 30)
                .put("active", true)
                .put("address", address)
                .put("tags", Arrays.asList("admin", "developer", "reviewer"));

        System.out.println("Built JSON:");
        System.out.println(user.toJsonString());

        // Read values
        System.out.println("\nReading values:");
        System.out.println("  name: " + user.getString("name"));
        System.out.println("  age: " + user.getInt("age", 0));
        System.out.println("  active: " + user.getBoolean("active", false));
        System.out.println("  tags: " + user.getArray("tags"));

        // Nested access via path
        System.out.println("\nDot-notation access:");
        System.out.println("  address.city: " + user.getPath("address.city"));
        System.out.println("  address.zip: " + user.getPath("address.zip"));

        // Flatten
        System.out.println("\nFlattened:");
        Map<String, Object> flat = user.flatten();
        for (Map.Entry<String, Object> entry : flat.entrySet()) {
            System.out.println("  " + entry.getKey() + " = " + entry.getValue());
        }

        // Modify
        user.put("age", 31).remove("active");
        System.out.println("\nAfter modification (age=31, removed 'active'):");
        System.out.println("  keys: " + user.keys());
        System.out.println("  size: " + user.size());
    }
}
