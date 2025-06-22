import { GameList } from "@/features/games";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard/games")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div>
      <GameList />
    </div>
  );
}
