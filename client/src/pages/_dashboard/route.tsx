import { Footer } from "@/components/Footer";
import { PrivateRoute } from "@/components/ProtectedRoute";
import { GamesHeader, SideBar } from "@/features/games";
import { useAuth } from "@/main";
import { createFileRoute, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard")({
  component: RouteComponent,
});

function RouteComponent() {
  const { isAuth } = useAuth();

  return (
    <PrivateRoute isAllowed={isAuth}>
      <div className="relative h-full flex flex-col">
        <div className="flex-1 flex flex-row">
          <aside className="flex-3/12 bg-gray-200">
            <SideBar />
          </aside>

          <div className="flex-9/12 flex flex-col bg-white">
            <GamesHeader />

            <Outlet />
          </div>
        </div>

        <Footer />
      </div>
    </PrivateRoute>
  );
}
