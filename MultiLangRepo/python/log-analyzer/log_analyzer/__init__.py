"""Log Analyzer - A tool for parsing and analyzing log files."""

from .analyzers.pattern_matcher import PatternMatcher
from .analyzers.stats_collector import StatsCollector
from .analyzers.log_parser import LogParser, LogEntry

import sys
from datetime import datetime


def main():
    """Demo entry point."""
    print("=== Log Analyzer Demo ===\n")

    sample_logs = [
        "2024-01-15 08:30:00 INFO  [main] Application started successfully",
        "2024-01-15 08:30:05 DEBUG [db] Connection pool initialized with 10 connections",
        "2024-01-15 08:31:12 WARN  [auth] Failed login attempt for user admin from 192.168.1.100",
        "2024-01-15 08:31:15 ERROR [auth] Account locked after 5 failed attempts: admin",
        "2024-01-15 08:32:00 INFO  [api] GET /api/users - 200 OK (45ms)",
        "2024-01-15 08:32:01 INFO  [api] POST /api/orders - 201 Created (120ms)",
        "2024-01-15 08:32:05 WARN  [api] GET /api/products - 429 Too Many Requests (2ms)",
        "2024-01-15 08:33:00 ERROR [db] Connection timeout after 30000ms",
        "2024-01-15 08:33:01 ERROR [db] Failed to execute query: SELECT * FROM orders WHERE status = 'pending'",
        "2024-01-15 08:34:00 INFO  [main] Scheduled maintenance starting",
        "2024-01-15 08:35:00 INFO  [main] Application shutdown complete",
    ]

    parser = LogParser()
    entries = [parser.parse(line) for line in sample_logs if parser.parse(line)]

    print(f"Parsed {len(entries)} log entries\n")

    matcher = PatternMatcher()
    errors = matcher.filter_by_level(entries, "ERROR")
    print(f"Errors found: {len(errors)}")
    for entry in errors:
        print(f"  [{entry.source}] {entry.message}")

    ip_matches = matcher.find_pattern(entries, r"\d+\.\d+\.\d+\.\d+")
    print(f"\nEntries with IP addresses: {len(ip_matches)}")

    collector = StatsCollector()
    stats = collector.compute(entries)
    print(f"\nLog Statistics:")
    print(f"  Total entries: {stats['total']}")
    print(f"  By level: {stats['by_level']}")
    print(f"  By source: {stats['by_source']}")
    print(f"  Time range: {stats['time_range']}")


if __name__ == "__main__":
    main()
