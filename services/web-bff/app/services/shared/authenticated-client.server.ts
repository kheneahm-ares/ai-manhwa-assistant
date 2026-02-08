import type { SessionUser } from '../auth.server';
import { createServiceClient } from './http-client.server';


/**
 * Creates service clients that are already authenticated with user context
 * This eliminates the need to pass userId/token to every service call
 */
export async function createAuthenticatedClients(user: SessionUser) {

  const userContext = {
    userId: user.id,
    accessToken: user.token,
  };

  return {
    readingProgress: createServiceClient(
      process.env.READING_PROGRESS_SERVICE_URL || 'http://localhost:5001',
      userContext
    ),
    userManagement: createServiceClient(
      process.env.USER_MANAGEMENT_SERVICE_URL || 'http://localhost:5000',
      userContext
    ),
  };
}
