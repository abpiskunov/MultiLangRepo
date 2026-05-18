"""Log parser module."""

import re
from dataclasses import dataclass
from datetime import datetime
from typing import Optional


@dataclass
class LogEntry:
    timestamp: datetime
    level: str
    source: str
    message: str
    raw: str


class LogParser:
    """Parses log lines into structured LogEntry objects."""

    PATTERN = re.compile(
        r"(?P<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\s+"
        r"(?P<level>\w+)\s+"
        r"\[(?P<source>\w+)\]\s+"
        r"(?P<message>.*)"
    )
    TIMESTAMP_FORMAT = "%Y-%m-%d %H:%M:%S"

    def parse(self, line: str) -> Optional[LogEntry]:
        match = self.PATTERN.match(line.strip())
        if not match:
            return None
        return LogEntry(
            timestamp=datetime.strptime(match.group("timestamp"), self.TIMESTAMP_FORMAT),
            level=match.group("level").upper(),
            source=match.group("source"),
            message=match.group("message"),
            raw=line.strip(),
        )

    def parse_many(self, lines: list[str]) -> list[LogEntry]:
        entries = []
        for line in lines:
            entry = self.parse(line)
            if entry:
                entries.append(entry)
        return entries

    def parse_file(self, filepath: str) -> list[LogEntry]:
        with open(filepath, "r", encoding="utf-8") as f:
            return self.parse_many(f.readlines())
