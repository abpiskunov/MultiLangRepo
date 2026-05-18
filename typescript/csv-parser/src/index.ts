export interface CsvParseOptions {
    delimiter?: string;
    quote?: string;
    hasHeader?: boolean;
    trimValues?: boolean;
    skipEmptyLines?: boolean;
}

export interface CsvRow {
    [key: string]: string;
}

export class CsvParser {
    private options: Required<CsvParseOptions>;

    constructor(options: CsvParseOptions = {}) {
        this.options = {
            delimiter: options.delimiter ?? ',',
            quote: options.quote ?? '"',
            hasHeader: options.hasHeader ?? true,
            trimValues: options.trimValues ?? true,
            skipEmptyLines: options.skipEmptyLines ?? true,
        };
    }

    parse(content: string): CsvRow[] {
        const lines = this.splitLines(content);
        if (lines.length === 0) return [];

        let headers: string[];
        let dataStart: number;

        if (this.options.hasHeader) {
            headers = this.parseLine(lines[0]);
            dataStart = 1;
        } else {
            const firstRow = this.parseLine(lines[0]);
            headers = firstRow.map((_, i) => `column_${i}`);
            dataStart = 0;
        }

        const rows: CsvRow[] = [];
        for (let i = dataStart; i < lines.length; i++) {
            const values = this.parseLine(lines[i]);
            const row: CsvRow = {};
            for (let j = 0; j < headers.length; j++) {
                row[headers[j]] = values[j] ?? '';
            }
            rows.push(row);
        }

        return rows;
    }

    parseToArrays(content: string): string[][] {
        const lines = this.splitLines(content);
        return lines.map(line => this.parseLine(line));
    }

    stringify(rows: CsvRow[], headers?: string[]): string {
        if (rows.length === 0) return '';

        const columnHeaders = headers ?? Object.keys(rows[0]);
        const lines: string[] = [];

        if (this.options.hasHeader) {
            lines.push(columnHeaders.map(h => this.escapeValue(h)).join(this.options.delimiter));
        }

        for (const row of rows) {
            const values = columnHeaders.map(h => this.escapeValue(row[h] ?? ''));
            lines.push(values.join(this.options.delimiter));
        }

        return lines.join('\n');
    }

    private splitLines(content: string): string[] {
        const lines = content.split(/\r?\n/);
        if (this.options.skipEmptyLines) {
            return lines.filter(line => line.trim().length > 0);
        }
        return lines;
    }

    private parseLine(line: string): string[] {
        const values: string[] = [];
        let current = '';
        let inQuotes = false;

        for (let i = 0; i < line.length; i++) {
            const char = line[i];

            if (inQuotes) {
                if (char === this.options.quote) {
                    if (i + 1 < line.length && line[i + 1] === this.options.quote) {
                        current += this.options.quote;
                        i++;
                    } else {
                        inQuotes = false;
                    }
                } else {
                    current += char;
                }
            } else {
                if (char === this.options.quote) {
                    inQuotes = true;
                } else if (char === this.options.delimiter) {
                    values.push(this.options.trimValues ? current.trim() : current);
                    current = '';
                } else {
                    current += char;
                }
            }
        }

        values.push(this.options.trimValues ? current.trim() : current);
        return values;
    }

    private escapeValue(value: string): string {
        if (value.includes(this.options.delimiter) ||
            value.includes(this.options.quote) ||
            value.includes('\n')) {
            const escaped = value.replace(
                new RegExp(this.options.quote, 'g'),
                this.options.quote + this.options.quote
            );
            return `${this.options.quote}${escaped}${this.options.quote}`;
        }
        return value;
    }
}

// Demo
function main(): void {
    const parser = new CsvParser();

    const csvContent = `Name,Age,City,Role
Alice,30,"New York",Engineer
Bob,25,"San Francisco","Product Manager"
Charlie,35,"Los Angeles, CA",Designer
"Diana ""Dee""",28,Chicago,Analyst`;

    console.log('=== CSV Parser Demo ===\n');
    console.log('Input:');
    console.log(csvContent);

    const rows = parser.parse(csvContent);
    console.log('\nParsed rows:');
    for (const row of rows) {
        console.log(`  ${row.Name} (${row.Age}) - ${row.City} [${row.Role}]`);
    }

    console.log('\nStringified back:');
    console.log(parser.stringify(rows));
}

main();
