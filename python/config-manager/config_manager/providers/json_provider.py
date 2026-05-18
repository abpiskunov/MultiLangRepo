"""JSON file configuration provider."""

import json
from typing import Any, Optional


class JsonProvider:
    def __init__(self, filepath: str):
        self._data: dict[str, Any] = {}
        self._load(filepath)

    def _load(self, filepath: str) -> None:
        with open(filepath, "r", encoding="utf-8") as f:
            raw = json.load(f)
        self._data = self._flatten(raw)

    def _flatten(self, data: dict, prefix: str = "") -> dict[str, Any]:
        result: dict[str, Any] = {}
        for key, value in data.items():
            full_key = f"{prefix}{key}" if not prefix else f"{prefix}.{key}"
            if isinstance(value, dict):
                result.update(self._flatten(value, full_key))
            else:
                result[full_key] = value
        return result

    def get(self, key: str) -> Optional[Any]:
        return self._data.get(key)

    def get_all(self) -> dict[str, Any]:
        return dict(self._data)
