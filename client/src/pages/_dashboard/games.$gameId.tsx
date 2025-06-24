import { createFileRoute, Navigate } from "@tanstack/react-router";
import { toast } from "react-toastify";
import { AxiosError } from "axios";

import { GameService } from "@features/games";
import { GameDetails } from "@features/games";

export const Route = createFileRoute("/_dashboard/games/$gameId")({
  component: RouteComponent,
  loader: ({ params }) => GameService.getGameById(params.gameId),
  pendingComponent: () => <div>Loading game details...</div>,
  errorComponent: ({ error }) => {
    if (error instanceof AxiosError) {
      const axiosError = error as AxiosError;

      if ("response" in axiosError) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const response = axiosError.response as any;
        if (response?.status === 404) {
          toast.error("Game not found");
          return <Navigate to="/games" replace />;
        } else {
          toast.error(
            `Error loading game details: ${response?.status} ${response.data.message}`
          );
          return <Navigate to="/games" replace />;
        }
      }
    }

    toast.error(`Error loading game details: ${error.message}`);
    return <Navigate to="/games" replace />;
  },
});

function RouteComponent() {
  return (
    <div>
      <GameDetails />
    </div>
  );
}
