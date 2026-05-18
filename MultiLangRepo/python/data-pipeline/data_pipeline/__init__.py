"""Data Pipeline - A simple ETL pipeline with pluggable transforms."""

from .transforms.filter_transform import FilterTransform
from .transforms.map_transform import MapTransform
from .transforms.aggregate_transform import AggregateTransform
from typing import Any, Callable


class Pipeline:
    def __init__(self, name: str = "default"):
        self.name = name
        self._steps: list[tuple[str, Callable]] = []

    def add_step(self, name: str, transform: Callable) -> "Pipeline":
        self._steps.append((name, transform))
        return self

    def execute(self, data: list[dict[str, Any]]) -> list[dict[str, Any]]:
        result = data
        for step_name, transform in self._steps:
            result = transform(result)
            print(f"  Step '{step_name}': {len(result)} records")
        return result


def main():
    print("=== Data Pipeline Demo ===\n")

    sales_data = [
        {"product": "Widget A", "category": "widgets", "quantity": 10, "price": 9.99, "region": "North"},
        {"product": "Widget B", "category": "widgets", "quantity": 5, "price": 14.99, "region": "South"},
        {"product": "Gadget X", "category": "gadgets", "quantity": 3, "price": 49.99, "region": "North"},
        {"product": "Gadget Y", "category": "gadgets", "quantity": 8, "price": 29.99, "region": "East"},
        {"product": "Widget C", "category": "widgets", "quantity": 0, "price": 7.99, "region": "West"},
        {"product": "Gadget Z", "category": "gadgets", "quantity": 12, "price": 19.99, "region": "North"},
        {"product": "Tool 1", "category": "tools", "quantity": 2, "price": 99.99, "region": "South"},
        {"product": "Tool 2", "category": "tools", "quantity": 7, "price": 59.99, "region": "East"},
    ]

    filter_t = FilterTransform()
    map_t = MapTransform()
    agg_t = AggregateTransform()

    pipeline = Pipeline("sales-analysis")
    pipeline.add_step("filter-nonzero", lambda data: filter_t.filter(data, "quantity", lambda q: q > 0))
    pipeline.add_step("add-total", lambda data: map_t.add_computed_field(data, "total", lambda r: r["quantity"] * r["price"]))

    print(f"Input: {len(sales_data)} records\n")
    print("Pipeline execution:")
    result = pipeline.execute(sales_data)

    print(f"\nResults:")
    for row in result:
        print(f"  {row['product']}: qty={row['quantity']}, total=${row['total']:.2f}")

    by_category = agg_t.group_by(result, "category")
    print(f"\nBy category:")
    for cat, items in by_category.items():
        total = sum(item["total"] for item in items)
        print(f"  {cat}: {len(items)} products, total=${total:.2f}")


if __name__ == "__main__":
    main()
