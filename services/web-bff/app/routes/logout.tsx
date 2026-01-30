import { redirect } from 'react-router';
import type { Route } from './+types/logout';
import { destroySessionCookie } from '~/services/auth.server';

export async function action({ request }: Route.ActionArgs) {
  return redirect('/login', {
    headers: {
      'Set-Cookie': destroySessionCookie(),
    },
  });
}

export async function loader() {
  throw redirect('/'); // Prevent direct GET access
}