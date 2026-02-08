import { Form, useActionData, redirect } from 'react-router';
import type { Route } from './+types/register';
import { createSessionCookie, getSession } from '~/services/auth.server';
import { userManagementService } from '~/services/user-management.server';
import { Card, CardHeader, CardTitle, CardContent } from '~/components/ui/card';
import { Input } from '~/components/ui/input';
import { Label } from '~/components/ui/label';
import { Button } from '~/components/ui/button';
import { Alert, AlertDescription } from '~/components/ui/alert';

export async function loader({ request }: Route.LoaderArgs) {
  // Redirect if already logged in
  const user = await getSession(request);
  if (user) throw redirect('/home');
  return null;
}

export async function action({ request }: Route.ActionArgs) {
  const formData = await request.formData();
  const email = formData.get('email') as string;
  const password = formData.get('password') as string;
  const displayName = formData.get('displayName') as string;

  try {
    const { accessToken } = await userManagementService.auth.register({
      email,
      password,
      displayName,
    });

    // Auto-login after registration
    return redirect('/home', {
      headers: {
        'Set-Cookie': createSessionCookie(accessToken),
      },
    });
  } catch (error) {
    return { error: 'Registration failed. Please try again.' };
  }
}

export default function Register({ actionData }: Route.ComponentProps) {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle className="text-2xl text-center">Register</CardTitle>
        </CardHeader>
        <CardContent>
          {actionData?.error && (
            <Alert variant="destructive" className="mb-4">
              <AlertDescription>{actionData.error}</AlertDescription>
            </Alert>
          )}
          <Form method="post" className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="displayName">Display Name</Label>
              <Input
                type="text"
                id="displayName"
                name="displayName"
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                type="email"
                id="email"
                name="email"
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input
                type="password"
                id="password"
                name="password"
                required
                minLength={6}
              />
            </div>
            <Button type="submit" className="w-full">
              Register
            </Button>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}