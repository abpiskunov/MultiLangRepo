"""Map transform for data pipeline."""

from typing import Any, Callable


class MapTransform:
    def add_computed_field(self, data: list[dict[str, Any]], field_name: str, compute: Callable) -> list[dict[str, Any]]:
        result = []
        for row in data:
            new_row = dict(row)
            new_row[field_name] = compute(row)
            result.append(new_row)
        return result

    def rename_field(self, data: list[dict[str, Any]], old_name: str, new_name: str) -> list[dict[str, Any]]:
        result = []
        for row in data:
            new_row = dict(row)
            if old_name in new_row:
                new_row[new_name] = new_row.pop(old_name)
            result.append(new_row)
        return result

    def select_fields(self, data: list[dict[str, Any]], fields: list[str]) -> list[dict[str, Any]]:
        return [{k: row.get(k) for k in fields} for row in data]

    def apply(self, data: list[dict[str, Any]], field: str, transform: Callable) -> list[dict[str, Any]]:
        result = []
        for row in data:
            new_row = dict(row)
            if field in new_row:
                new_row[field] = transform(new_row[field])
            result.append(new_row)
        return result
