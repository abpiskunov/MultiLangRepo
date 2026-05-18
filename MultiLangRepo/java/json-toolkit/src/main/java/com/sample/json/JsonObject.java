package com.sample.json;

import java.util.*;

/**
 * A simple JSON-like object builder and manipulator.
 * Stores data as nested Maps and Lists without external dependencies.
 */
public class JsonObject {
    private final Map<String, Object> data;

    public JsonObject() {
        this.data = new LinkedHashMap<>();
    }

    public JsonObject(Map<String, Object> data) {
        this.data = new LinkedHashMap<>(data);
    }

    public JsonObject put(String key, Object value) {
        data.put(key, value);
        return this;
    }

    public Object get(String key) {
        return data.get(key);
    }

    public String getString(String key) {
        Object value = data.get(key);
        return value != null ? value.toString() : null;
    }

    public int getInt(String key, int defaultValue) {
        Object value = data.get(key);
        if (value instanceof Number) {
            return ((Number) value).intValue();
        }
        if (value instanceof String) {
            try {
                return Integer.parseInt((String) value);
            } catch (NumberFormatException e) {
                return defaultValue;
            }
        }
        return defaultValue;
    }

    public boolean getBoolean(String key, boolean defaultValue) {
        Object value = data.get(key);
        if (value instanceof Boolean) {
            return (Boolean) value;
        }
        if (value instanceof String) {
            return "true".equalsIgnoreCase((String) value);
        }
        return defaultValue;
    }

    @SuppressWarnings("unchecked")
    public JsonObject getObject(String key) {
        Object value = data.get(key);
        if (value instanceof Map) {
            return new JsonObject((Map<String, Object>) value);
        }
        return null;
    }

    @SuppressWarnings("unchecked")
    public List<Object> getArray(String key) {
        Object value = data.get(key);
        if (value instanceof List) {
            return (List<Object>) value;
        }
        return null;
    }

    public boolean has(String key) {
        return data.containsKey(key);
    }

    public JsonObject remove(String key) {
        data.remove(key);
        return this;
    }

    public Set<String> keys() {
        return data.keySet();
    }

    public int size() {
        return data.size();
    }

    /**
     * Get a nested value using dot-notation path (e.g., "user.address.city").
     */
    @SuppressWarnings("unchecked")
    public Object getPath(String path) {
        String[] parts = path.split("\\.");
        Object current = data;

        for (String part : parts) {
            if (current instanceof Map) {
                current = ((Map<String, Object>) current).get(part);
            } else {
                return null;
            }
        }
        return current;
    }

    /**
     * Flatten nested structure into dot-notation keys.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> flatten() {
        Map<String, Object> result = new LinkedHashMap<>();
        flattenHelper("", data, result);
        return result;
    }

    @SuppressWarnings("unchecked")
    private void flattenHelper(String prefix, Map<String, Object> map, Map<String, Object> result) {
        for (Map.Entry<String, Object> entry : map.entrySet()) {
            String key = prefix.isEmpty() ? entry.getKey() : prefix + "." + entry.getKey();
            if (entry.getValue() instanceof Map) {
                flattenHelper(key, (Map<String, Object>) entry.getValue(), result);
            } else {
                result.put(key, entry.getValue());
            }
        }
    }

    /**
     * Simple JSON serialization (no external library).
     */
    public String toJsonString() {
        return toJsonString(data, 0);
    }

    @SuppressWarnings("unchecked")
    private String toJsonString(Object obj, int indent) {
        if (obj == null) return "null";
        if (obj instanceof String) return "\"" + escapeString((String) obj) + "\"";
        if (obj instanceof Number || obj instanceof Boolean) return obj.toString();

        if (obj instanceof Map) {
            Map<String, Object> map = (Map<String, Object>) obj;
            if (map.isEmpty()) return "{}";
            StringBuilder sb = new StringBuilder();
            sb.append("{\n");
            int i = 0;
            for (Map.Entry<String, Object> entry : map.entrySet()) {
                sb.append(spaces(indent + 2));
                sb.append("\"").append(escapeString(entry.getKey())).append("\": ");
                sb.append(toJsonString(entry.getValue(), indent + 2));
                if (i++ < map.size() - 1) sb.append(",");
                sb.append("\n");
            }
            sb.append(spaces(indent)).append("}");
            return sb.toString();
        }

        if (obj instanceof List) {
            List<Object> list = (List<Object>) obj;
            if (list.isEmpty()) return "[]";
            StringBuilder sb = new StringBuilder();
            sb.append("[\n");
            for (int i = 0; i < list.size(); i++) {
                sb.append(spaces(indent + 2));
                sb.append(toJsonString(list.get(i), indent + 2));
                if (i < list.size() - 1) sb.append(",");
                sb.append("\n");
            }
            sb.append(spaces(indent)).append("]");
            return sb.toString();
        }

        return "\"" + obj.toString() + "\"";
    }

    private String escapeString(String s) {
        return s.replace("\\", "\\\\")
                .replace("\"", "\\\"")
                .replace("\n", "\\n")
                .replace("\t", "\\t");
    }

    private String spaces(int count) {
        return " ".repeat(count);
    }

    @Override
    public String toString() {
        return toJsonString();
    }
}
