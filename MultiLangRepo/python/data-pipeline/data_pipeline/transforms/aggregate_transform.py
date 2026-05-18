"""Aggregate transform for data pipeline."""

from typing import Any
from collections import defaultdict


class AggregateTransform:
    def group_by(self, data: list[dict[str, Any]], field: str) -> dict[Any, list[dict[str, Any]]]:
        groups: dict[Any, list[dict[str, Any]]] = defaultdict(list)
        for row in data:
            groups[row.get(field, "unknown")].append(row)
        return dict(groups)

    def sum_by(self, data: list[dict[str, Any]], group_field: str, sum_field: str) -> dict[Any, float]:
        groups = self.group_by(data, group_field)
        return {key: sum(row.get(sum_field, 0) for row in rows) for key, rows in groups.items()}

    def count_by(self, data: list[dict[str, Any]], field: str) -> dict[Any, int]:
        groups = self.group_by(data, field)
        return {key: len(rows) for key, rows in groups.items()}

    def avg_by(self, data: list[dict[str, Any]], group_field: str, avg_field: str) -> dict[Any, float]:
        groups = self.group_by(data, group_field)
        result: dict[Any, float] = {}
        for key, rows in groups.items():
            values = [row.get(avg_field, 0) for row in rows]
            result[key] = sum(values) / len(values) if values else 0
        return result
