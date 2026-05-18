import * as https from 'https';
import * as http from 'http';

export interface HttpResponse {
    statusCode: number;
    headers: Record<string, string | string[] | undefined>;
    body: string;
    elapsed: number;
}

export interface RetryConfig {
    maxRetries: number;
    baseDelay: number;
    maxDelay: number;
}

export interface CacheEntry {
    response: HttpResponse;
    expiresAt: number;
}

export class HttpClient {
    private cache: Map<string, CacheEntry> = new Map();
    private defaultTimeout: number;
    private retryConfig: RetryConfig;

    constructor(timeout: number = 10000, retryConfig?: Partial<RetryConfig>) {
        this.defaultTimeout = timeout;
        this.retryConfig = {
            maxRetries: retryConfig?.maxRetries ?? 3,
            baseDelay: retryConfig?.baseDelay ?? 1000,
            maxDelay: retryConfig?.maxDelay ?? 10000,
        };
    }

    async get(url: string, cacheTtl?: number): Promise<HttpResponse> {
        if (cacheTtl) {
            const cached = this.getFromCache(url);
            if (cached) return cached;
        }

        const response = await this.requestWithRetry(url, 'GET');

        if (cacheTtl && response.statusCode === 200) {
            this.setCache(url, response, cacheTtl);
        }

        return response;
    }

    async post(url: string, body: string, contentType: string = 'application/json'): Promise<HttpResponse> {
        return this.requestWithRetry(url, 'POST', body, contentType);
    }

    clearCache(): void {
        this.cache.clear();
    }

    getCacheSize(): number {
        this.evictExpired();
        return this.cache.size;
    }

    private getFromCache(key: string): HttpResponse | null {
        const entry = this.cache.get(key);
        if (!entry) return null;
        if (Date.now() > entry.expiresAt) {
            this.cache.delete(key);
            return null;
        }
        return entry.response;
    }

    private setCache(key: string, response: HttpResponse, ttl: number): void {
        this.cache.set(key, {
            response,
            expiresAt: Date.now() + ttl,
        });
    }

    private evictExpired(): void {
        const now = Date.now();
        for (const [key, entry] of this.cache.entries()) {
            if (now > entry.expiresAt) {
                this.cache.delete(key);
            }
        }
    }

    private async requestWithRetry(url: string, method: string, body?: string, contentType?: string): Promise<HttpResponse> {
        let lastError: Error | undefined;

        for (let attempt = 0; attempt <= this.retryConfig.maxRetries; attempt++) {
            try {
                return await this.makeRequest(url, method, body, contentType);
            } catch (error) {
                lastError = error as Error;
                if (attempt < this.retryConfig.maxRetries) {
                    const delay = Math.min(
                        this.retryConfig.baseDelay * Math.pow(2, attempt),
                        this.retryConfig.maxDelay
                    );
                    await this.sleep(delay);
                }
            }
        }

        throw lastError;
    }

    private makeRequest(url: string, method: string, body?: string, contentType?: string): Promise<HttpResponse> {
        return new Promise((resolve, reject) => {
            const startTime = Date.now();
            const isHttps = url.startsWith('https');
            const lib = isHttps ? https : http;

            const options: http.RequestOptions = {
                method,
                timeout: this.defaultTimeout,
                headers: contentType ? { 'Content-Type': contentType } : undefined,
            };

            const req = lib.request(url, options, (res) => {
                let data = '';
                res.on('data', (chunk) => { data += chunk; });
                res.on('end', () => {
                    resolve({
                        statusCode: res.statusCode ?? 0,
                        headers: res.headers as Record<string, string | string[] | undefined>,
                        body: data,
                        elapsed: Date.now() - startTime,
                    });
                });
            });

            req.on('error', reject);
            req.on('timeout', () => {
                req.destroy();
                reject(new Error(`Request timed out after ${this.defaultTimeout}ms`));
            });

            if (body) {
                req.write(body);
            }
            req.end();
        });
    }

    private sleep(ms: number): Promise<void> {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

// Demo
async function main(): Promise<void> {
    const client = new HttpClient(5000, { maxRetries: 2 });

    console.log('=== HTTP Client Demo ===\n');

    try {
        console.log('Fetching httpbin.org/get (with 30s cache)...');
        const response = await client.get('https://httpbin.org/get', 30000);
        console.log(`Status: ${response.statusCode}`);
        console.log(`Elapsed: ${response.elapsed}ms`);
        console.log(`Body length: ${response.body.length} chars`);
        console.log(`Cache size: ${client.getCacheSize()}`);

        console.log('\nFetching same URL again (should be cached)...');
        const cached = await client.get('https://httpbin.org/get', 30000);
        console.log(`Elapsed: ${cached.elapsed}ms (cached)`);
    } catch (error) {
        console.error('Request failed:', (error as Error).message);
    }
}

main();
