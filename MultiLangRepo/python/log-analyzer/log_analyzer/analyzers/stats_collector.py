"""Statistics collector for log entries."""

from collections import Counter
from .log_parser import LogEntry


class StatsCollector:
    """Computes statistics over a collection of log entries."""

    def compute(self, entries: list[LogEntry]) -> dict:
        if not entries:
            return {"total": 0, "by_level": {}, "by_source": {}, "time_range": None}

        by_level = Counter(e.level for e in entries)
        by_source = Counter(e.source for e in entries)
        timestamps = [e.timestamp for e in entries]
        min_ts, max_ts = min(timestamps), max(timestamps)

        return {
            "total": len(entries),
            "by_level": dict(by_level),
            "by_source": dict(by_source),
            "time_range": f"{min_ts} to {max_ts} ({max_ts - min_ts})",
            "entries_per_minute": self._entries_per_minute(entries),
            "error_rate": by_level.get("ERROR", 0) / len(entries) * 100,
        }

    def _entries_per_minute(self, entries: list[LogEntry]) -> float:
        if len(entries) < 2:
            return 0.0
        timestamps = sorted(e.timestamp for e in entries)
        duration = (timestamps[-1] - timestamps[0]).total_seconds()
        if duration == 0:
            return float(len(entries))
        return len(entries) / (duration / 60)

    def top_sources(self, entries: list[LogEntry], n: int = 5) -> list[tuple[str, int]]:
        return Counter(e.source for e in entries).most_common(n)

    def error_summary(self, entries: list[LogEntry]) -> dict[str, list[str]]:
        result: dict[str, list[str]] = {}
        for entry in entries:
            if entry.level == "ERROR":
                result.setdefault(entry.source, []).append(entry.message)
        return result
