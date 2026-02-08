/**
 * User Management Service Client
 * Handles authentication and user management operations
 */

import { createServiceClient } from './shared/http-client.server';

const BASE_URL = process.env.USER_SERVICE_URL;

if (!BASE_URL) {
  throw new Error('USER_SERVICE_URL environment variable is required');
}

const client = createServiceClient(BASE_URL);

// Types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  displayName: string;
}

export interface RegisterResponse {
  accessToken: string;
}

export interface User {
  id: string;
  email: string;
  displayName: string;
  createdAt: string;
}

/**
 * User Management Service API
 */
export const userManagementService = {
  /**
   * Authentication operations
   */
  auth: {
    /**
     * Authenticate user with email and password
     */
    login: async (credentials: LoginRequest): Promise<LoginResponse> => {
      return client.post<LoginResponse>('/auth/login', credentials);
    },

    /**
     * Register a new user account
     */
    register: async (data: RegisterRequest): Promise<RegisterResponse> => {
      return client.post<RegisterResponse>('/auth/register', data);
    },
  },

  /**
   * User profile operations
   */
  users: {
    /**
     * Get user profile by ID
     */
    getProfile: async (userId: string): Promise<User> => {
      return client.get<User>(`/users/${userId}`);
    },

    /**
     * Update user profile
     */
    updateProfile: async (
      userId: string,
      updates: Partial<Pick<User, 'displayName' | 'email'>>
    ): Promise<User> => {
      return client.put<User>(`/users/${userId}`, updates, { userId });
    },
  },
};
