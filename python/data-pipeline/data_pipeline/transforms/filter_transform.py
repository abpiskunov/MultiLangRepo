"""Filter transform for data pipeline."""

from typing import Any, Callable


class FilterTransform:
    def filter(self, data: list[dict[str, Any]], field: str, predicate: Callable) -> list[dict[str, Any]]:
        return [row for row in data if field in row and predicate(row[field])]

    def filter_by_values(self, data: list[dict[str, Any]], field: str, values: set[Any]) -> list[dict[str, Any]]:
        return [row for row in data if row.get(field) in values]

    def exclude_by_values(self, data: list[dict[str, Any]], field: str, values: set[Any]) -> list[dict[str, Any]]:
        return [row for row in data if row.get(field) not in values]

    def filter_not_null(self, data: list[dict[str, Any]], field: str) -> list[dict[str, Any]]:
        return [row for row in data if row.get(field) is not None]

    def deduplicate(self, data: list[dict[str, Any]], key_field: str) -> list[dict[str, Any]]:
        seen: set[Any] = set()
        result: list[dict[str, Any]] = []
        for row in data:
            key = row.get(key_field)
            if key not in seen:
                seen.add(key)
                result.append(row)
        return result
