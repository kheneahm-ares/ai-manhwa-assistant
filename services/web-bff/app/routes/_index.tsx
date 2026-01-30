import { Link } from 'react-router';
import type { Route } from './+types/_index';
import { getSession } from '~/services/auth.server';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from '~/components/ui/card';
import { Button } from '~/components/ui/button';

export async function loader({ request }: Route.LoaderArgs) {
  // Check if user is already logged in
  const user = await getSession(request);
  return { user };
}

export default function Index({ loaderData }: Route.ComponentProps) {
  const { user } = loaderData;

  if (user) {
    // Logged in users see different content
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
        <Card className="w-full max-w-md">
          <CardHeader>
            <CardTitle className="text-2xl">Welcome back, {user.displayName}!</CardTitle>
          </CardHeader>
          <CardContent>
            <Button asChild className="w-full">
              <Link to="/home">Go to Dashboard</Link>
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  // Public landing page
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle className="text-3xl">Manhwa Reading Assistant</CardTitle>
          <CardDescription>Track your manhwa reading progress</CardDescription>
        </CardHeader>
        <CardContent className="flex gap-4">
          <Button asChild className="flex-1">
            <Link to="/login">Login</Link>
          </Button>
          <Button asChild variant="outline" className="flex-1">
            <Link to="/register">Register</Link>
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}