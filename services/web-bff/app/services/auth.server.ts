import { redirect } from 'react-router';
import * as cookie from 'cookie';
import { jwtVerify } from 'jose'; // Use jose for JWT verification

export interface SessionUser {
  id: string;
  email: string;
  displayName: string;
  token: string;
}

const JWT_SECRET = new TextEncoder().encode(process.env.JWT_SECRET);
const COOKIE_NAME = 'manhwa_session';

export async function getSession(request: Request): Promise<SessionUser | null> {
  const cookieHeader = request.headers.get('Cookie');
  console.log('🍪 Cookie header:', cookieHeader); // DEBUG
  
  if (!cookieHeader) {
    console.log('❌ No cookie header found');
    return null;
  }

  const cookies = cookie.parse(cookieHeader);
  console.log('📦 Parsed cookies:', cookies); // DEBUG
  
  const token = cookies[COOKIE_NAME];
  console.log('🎫 Token:', token ? 'EXISTS' : 'MISSING'); // DEBUG
  
  if (!token) return null;

  try {
    const { payload } = await jwtVerify(token, JWT_SECRET);
    console.log('✅ JWT verified:', payload); // DEBUG
    
    return {
      id: payload.sub as string,
      email: payload.email as string,
      displayName: payload.displayName as string,
      token: token,
    };
  } catch (error) {
    console.log('❌ JWT verification failed:', error); // DEBUG
    return null;
  }
}

export async function requireSession(request: Request): Promise<SessionUser> {
  const user = await getSession(request);
  if (!user) throw redirect('/login');
  return user;
}

export function createSessionCookie(token: string): string {
  return cookie.serialize(COOKIE_NAME, token, {
    httpOnly: true,
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'lax',
    maxAge: 60 * 60 * 24 * 7, // 7 days
    path: '/',
  });
}

export function destroySessionCookie(): string {
  return cookie.serialize(COOKIE_NAME, '', {
    httpOnly: true,
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'lax',
    maxAge: 0,
    path: '/',
  });
}