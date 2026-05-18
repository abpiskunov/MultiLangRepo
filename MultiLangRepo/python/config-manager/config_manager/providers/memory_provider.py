"""In-memory configuration provider."""

from typing import Any, Optional


class MemoryProvider:
    def __init__(self, data: Optional[dict[str, Any]] = None):
        self._data: dict[str, Any] = dict(data) if data else {}

    def get(self, key: str) -> Optional[Any]:
        return self._data.get(key)

    def set(self, key: str, value: Any) -> None:
        self._data[key] = value

    def delete(self, key: str) -> bool:
        if key in self._data:
            del self._data[key]
            return True
        return False

    def get_all(self) -> dict[str, Any]:
        return dict(self._data)
