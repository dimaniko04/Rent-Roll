import React from "react";
import { GameService } from "@/features/games/services/GameService";
import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard/games/$gameId")({
  component: RouteComponent,
  loader: ({ params }) => GameService.getGameById(params.gameId),
});

function RouteComponent() {
  return (
    <React.Suspense fallback={<div>Loading...</div>}>
      <nav style={{ marginBottom: 16 }}>
        <p>
          <Link to="/games">Home</Link> / Details game
        </p>
      </nav>
    </React.Suspense>
  );
}
