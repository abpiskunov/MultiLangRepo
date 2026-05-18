"""Environment variable configuration provider."""

import os
from typing import Any, Optional


class EnvProvider:
    def __init__(self, prefix: str = ""):
        self._prefix = prefix

    def get(self, key: str) -> Optional[str]:
        env_key = self._prefix + key.replace(".", "_").upper()
        return os.environ.get(env_key)

    def get_all(self) -> dict[str, str]:
        result: dict[str, str] = {}
        for key, value in os.environ.items():
            if self._prefix and key.startswith(self._prefix):
                config_key = key[len(self._prefix):].lower().replace("_", ".")
                result[config_key] = value
        return result
