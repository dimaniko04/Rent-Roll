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
      <div className="flex flex-col h-full">
        <div className="flex flex-1 overflow-y-auto flex-row">
          <aside className="flex-3/12 max-w-64 bg-gray-200 overflow-y-auto">
            <SideBar />
          </aside>

          <div className="flex-9/12 flex flex-col overflow-y-auto bg-white">
            <GamesHeader />

            <div className="flex-1 overflow-y-auto">
              <Outlet />
            </div>
          </div>
        </div>

        <Footer />
      </div>
    </PrivateRoute>
  );
}
