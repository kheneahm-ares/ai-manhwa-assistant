/**
 * Reading Progress Service Client
 * Handles reading lists and progress tracking operations
 */

import type { IServiceClient } from './shared/http-client.server';

// Types
export interface ReadingList {
  manhwaId: string;
  description?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateReadingListRequest {
  manhwaId: string;
}

export interface UpdateReadingListRequest {
  manhwaId?: string;
  status?: string;
}

export interface ProgressEvent {
  id: string;
  userId: string;
  manhwaId: string;
  chapterNumber: number;
  pageNumber?: number;
  timestamp: string;
}

export interface AddProgressEventRequest {
  manhwaId: string;
  chapterNumber: number;
  pageNumber?: number;
}

export interface ProgressEventFilters {
  manhwaId?: string;
  limit?: number;
  offset?: number;
}

/**
 * Reading Progress Service API
 * Pass an authenticated client from createAuthenticatedClients()
 */
export function createReadingProgressService(client: IServiceClient) {
  return {
    /**
     * Reading Lists operations
     */
    readingLists: {
      /**
       * Get all reading lists for a user
       */
      getAll: async (): Promise<ReadingList[]> => {
        return client.get<ReadingList[]>('/reading-progress/reading-lists');
      },

      /**
       * Create a new reading list
       */
      create: async (data: CreateReadingListRequest): Promise<ReadingList> => {
        return client.post<ReadingList>('/reading-progress/reading-lists', data);
      },

      /**
       * Update an existing reading list
       */
      update: async (data: UpdateReadingListRequest): Promise<ReadingList> => {
        return client.put<ReadingList>(`/reading-progress/reading-lists`, data);
      },
    },

    /**
     * Reading Progress Events operations
     */
    progressEvents: {
      /**
       * Add a new progress event (user read a chapter)
       */
      add: async (data: AddProgressEventRequest): Promise<ProgressEvent> => {
        return client.post<ProgressEvent>('/reading-progress/reading-progress-events', data);
      },
    },
  };
}
