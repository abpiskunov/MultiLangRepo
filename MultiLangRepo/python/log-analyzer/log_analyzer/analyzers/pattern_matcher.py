"""Pattern matching analyzer."""

import re
from typing import Optional
from .log_parser import LogEntry


class PatternMatcher:
    """Filters and searches log entries by patterns."""

    def filter_by_level(self, entries: list[LogEntry], level: str) -> list[LogEntry]:
        return [e for e in entries if e.level == level.upper()]

    def filter_by_source(self, entries: list[LogEntry], source: str) -> list[LogEntry]:
        return [e for e in entries if e.source == source]

    def find_pattern(self, entries: list[LogEntry], pattern: str) -> list[LogEntry]:
        compiled = re.compile(pattern)
        return [e for e in entries if compiled.search(e.message)]

    def find_keyword(self, entries: list[LogEntry], keyword: str, case_sensitive: bool = False) -> list[LogEntry]:
        if case_sensitive:
            return [e for e in entries if keyword in e.message]
        keyword_lower = keyword.lower()
        return [e for e in entries if keyword_lower in e.message.lower()]

    def find_between(self, entries: list[LogEntry], start_pattern: str, end_pattern: str) -> list[list[LogEntry]]:
        sequences: list[list[LogEntry]] = []
        current: Optional[list[LogEntry]] = None
        start_re = re.compile(start_pattern)
        end_re = re.compile(end_pattern)

        for entry in entries:
            if start_re.search(entry.message):
                current = [entry]
            elif current is not None:
                current.append(entry)
                if end_re.search(entry.message):
                    sequences.append(current)
                    current = None
        return sequences
