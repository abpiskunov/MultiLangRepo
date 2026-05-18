"""Config Manager - Hierarchical configuration with multiple providers."""

from .providers.json_provider import JsonProvider
from .providers.env_provider import EnvProvider
from .providers.memory_provider import MemoryProvider
from typing import Any, Optional


class ConfigManager:
    def __init__(self):
        self._providers: list[tuple[str, Any]] = []

    def add_provider(self, name: str, provider: Any) -> "ConfigManager":
        self._providers.append((name, provider))
        return self

    def get(self, key: str, default: Any = None) -> Any:
        for _, provider in reversed(self._providers):
            value = provider.get(key)
            if value is not None:
                return value
        return default

    def get_required(self, key: str) -> Any:
        value = self.get(key)
        if value is None:
            raise KeyError(f"Required configuration key '{key}' not found")
        return value

    def get_int(self, key: str, default: int = 0) -> int:
        value = self.get(key)
        return int(value) if value is not None else default

    def get_bool(self, key: str, default: bool = False) -> bool:
        value = self.get(key)
        if value is None:
            return default
        if isinstance(value, bool):
            return value
        return str(value).lower() in ("true", "1", "yes", "on")

    def get_all(self) -> dict[str, Any]:
        merged: dict[str, Any] = {}
        for _, provider in self._providers:
            merged.update(provider.get_all())
        return merged

    def list_providers(self) -> list[str]:
        return [name for name, _ in self._providers]


def main():
    print("=== Config Manager Demo ===\n")
    memory = MemoryProvider({
        "app.name": "MultiLangRepo",
        "app.version": "1.0.0",
        "database.host": "localhost",
        "database.port": "5432",
        "debug": "false",
        "max_connections": "10",
    })
    env = EnvProvider(prefix="APP_")
    config = ConfigManager()
    config.add_provider("defaults", memory)
    config.add_provider("environment", env)

    print(f"Providers: {config.list_providers()}")
    print(f"app.name = {config.get('app.name')}")
    print(f"database.port (int) = {config.get_int('database.port')}")
    print(f"debug (bool) = {config.get_bool('debug')}")
    print(f"missing = {config.get('missing', 'default_value')}")


if __name__ == "__main__":
    main()
