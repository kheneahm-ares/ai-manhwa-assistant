/**
 * Shared HTTP client for service-to-service communication
 * Provides consistent error handling, headers, and request patterns
 */

export class ServiceError extends Error {
  constructor(
    public status: number,
    message: string,
    public details?: unknown
  ) {
    super(message);
    this.name = 'ServiceError';
  }
}

interface RequestContext {
  userId?: string;
  accessToken?: string;
  headers?: Record<string, string>;
}

export interface IServiceClient {
  get<T>(path: string, context?: RequestContext): Promise<T>;
  post<T>(path: string, data?: unknown, context?: RequestContext): Promise<T>;
    put<T>(path: string, data?: unknown, context?: RequestContext): Promise<T>;
    delete<T>(path: string, context?: RequestContext): Promise<T>;
    patch<T>(path: string, data?: unknown, context?: RequestContext): Promise<T>;
}

export function createServiceClient(baseUrl: string, context?: RequestContext) : IServiceClient {
  async function request<T>(
    path: string,
    options: RequestInit,
  ): Promise<T> {
    const url = new URL("/api" + path, baseUrl);
    const headers = new Headers(options.headers);

    // Set default content type
    if (!headers.has('Content-Type')) {
      headers.set('Content-Type', 'application/json');
    }

    // Add JWT token for authentication with microservices
    if (context?.accessToken) {
      headers.set('Authorization', `Bearer ${context.accessToken}`);
    }

    try {
        console.log(`➡️ Requesting ${url.toString()} with options:`, { options, headers }); // DEBUG
      const response = await fetch(url.toString(), {
        ...options,
        headers,
      });

      if (!response.ok) {
        const errorText = await response.text();
        let errorDetails;
        try {
          errorDetails = JSON.parse(errorText);
        } catch {
          errorDetails = errorText;
        }
        throw new ServiceError(
          response.status,
          errorDetails.message || `Service request failed: ${response.statusText}`,
          errorDetails
        );
      }

      // Handle empty responses
      const contentType = response.headers.get('Content-Type');
      if (!contentType || !contentType.includes('application/json')) {
        return {} as T;
      }

      return await response.json();
    } catch (error) {
      if (error instanceof ServiceError) {
        throw error;
      }
      throw new ServiceError(
        500,
        error instanceof Error ? error.message : 'Unknown error occurred'
      );
    }
  }

  return {
    async get<T>(path: string): Promise<T> {
      return request<T>(path, { method: 'GET' });
    },

    async post<T>(
      path: string,
      data?: unknown
    ): Promise<T> {
      return request<T>(
        path,
        {
          method: 'POST',
          body: data ? JSON.stringify(data) : undefined,
        },
      );
    },

    async put<T>(
      path: string,
      data?: unknown
    ): Promise<T> {
      return request<T>(
        path,
        {
          method: 'PUT',
          body: data ? JSON.stringify(data) : undefined,
        },
      );
    },

    async delete<T>(path: string): Promise<T> {
      return request<T>(path, { method: 'DELETE' });
    },

    async patch<T>(
      path: string,
      data?: unknown
    ): Promise<T> {
      return request<T>(
        path,
        {
          method: 'PATCH',
          body: data ? JSON.stringify(data) : undefined,
        },
      );
    },
  };
}
