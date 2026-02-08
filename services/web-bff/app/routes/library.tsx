import { createAuthenticatedClients } from '~/services/shared/authenticated-client.server';
import type { Route } from './+types/library';
import { requireSession } from '~/services/auth.server';
import { createReadingProgressService } from '~/services/reading-progress.server';

export async function loader({ request }: Route.LoaderArgs) {
    
  const user = await requireSession(request);
  const { readingProgress } = await createAuthenticatedClients(user);

    const readingProgressService = createReadingProgressService(readingProgress);

  const [readingLists] = await Promise.all([
    readingProgressService.readingLists.getAll(),
  ]);

  console.log('📚 Fetched reading lists:', readingLists); // DEBUG

  return Response.json({
    user: {
      displayName: user.displayName,
    },
    library: {
      lists: readingLists,
    },
  });
}

/**
 * Action demonstrates handling mutations through BFF
 */
export async function action({ request }: Route.ActionArgs) {
  const user = await requireSession(request);
  const formData = await request.formData();
  const intent = formData.get('intent');

    const { readingProgress } = await createAuthenticatedClients(user);
    const readingProgressService = createReadingProgressService(readingProgress);


  if (intent === 'create-list') {
    const manhwaId = formData.get('manhwaId') as string;

    await readingProgressService.readingLists.create({
      manhwaId,
    });

    return Response.json({ success: true });
  }

  return Response.json({ error: 'Unknown intent' }, { status: 400 });
}

export default function Library({ loaderData }: Route.ComponentProps) {
  return (
    <div>
      <h1>Welcome, {loaderData.user.displayName}</h1>
      
      <section>
        <h2>Your Reading Lists</h2>
        <ul>
          {loaderData.library.lists.map((list) => (
            <li key={list.manhwaId}>
              <strong>{list.manhwaId}</strong> - Created at: {new Date(list.startDate).toLocaleDateString()}
            </li>
          ))}
        </ul>
      </section>
    </div>
  );
}
