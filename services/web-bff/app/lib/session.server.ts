import { createCookieSessionStorage, redirect } from "react-router";

// Session data type
export interface SessionData {
  userId: string;
  email: string;
  displayName: string;
  accessToken: string;
}

// Create session storage
const sessionSecret = process.env.SESSION_SECRET;
if (!sessionSecret) {
  throw new Error("SESSION_SECRET must be set");
}

const { getSession, commitSession, destroySession } =
  createCookieSessionStorage<SessionData>({
    cookie: {
      name: "__session",
      httpOnly: true,
      maxAge: 60 * 60 * 24 * 7, // 7 days
      path: "/",
      sameSite: "lax",
      secrets: [sessionSecret],
      secure: process.env.NODE_ENV === "production",
    },
  });

export { getSession, commitSession, destroySession };

// Helper to get user session
export async function getUserSession(request: Request) {
  const session = await getSession(request.headers.get("Cookie"));
  return session;
}

// Helper to get user or redirect to login
export async function requireUser(request: Request) {
  const session = await getUserSession(request);
  const userId = session.get("userId");
  const accessToken = session.get("accessToken");

  if (!userId || !accessToken) {
    throw redirect("/login");
  }

  return {
    userId,
    email: session.get("email"),
    displayName: session.get("displayName"),
    accessToken,
  };
}

// Helper to create user session
export async function createUserSession({
  request,
  userId,
  email,
  displayName,
  accessToken,
  redirectTo,
}: {
  request: Request;
  userId: string;
  email: string;
  displayName: string;
  accessToken: string;
  redirectTo: string;
}) {
  const session = await getSession(request.headers.get("Cookie"));
  session.set("userId", userId);
  session.set("email", email);
  session.set("displayName", displayName);
  session.set("accessToken", accessToken);

  return redirect(redirectTo, {
    headers: {
      "Set-Cookie": await commitSession(session),
    },
  });
}

// Helper to logout
export async function logout(request: Request) {
  const session = await getUserSession(request);
  return redirect("/login", {
    headers: {
      "Set-Cookie": await destroySession(session),
    },
  });
}
