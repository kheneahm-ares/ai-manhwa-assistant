import { Form } from 'react-router';
import type { Route } from './+types/home';
import { requireSession } from '~/services/auth.server';
import { Card, CardHeader, CardTitle, CardContent } from '~/components/ui/card';
import { Button } from '~/components/ui/button';

export async function loader({ request }: Route.LoaderArgs) {
  // THIS IS THE ROUTE GUARD - throws redirect to /login if not authenticated
  const user = await requireSession(request);
  
  return { user };
}

export default function Home({ loaderData }: Route.ComponentProps) {
  const { user } = loaderData;

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 p-8">
      <header className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Hi {user.displayName}!</h1>
        <Form method="post" action="/logout">
          <Button type="submit" variant="destructive">
            Logout
          </Button>
        </Form>
      </header>

      <Card>
        <CardHeader>
          <CardTitle>Your Dashboard</CardTitle>
        </CardHeader>
        <CardContent className="space-y-2">
          <p className="text-sm">Email: {user.email}</p>
          <p className="text-sm">User ID: {user.id}</p>
          {/* Future: Reading progress, recommendations, etc. */}
        </CardContent>
      </Card>
    </div>
  );
}