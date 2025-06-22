import { PrivateRoute } from "@/components/ProtectedRoute";
import { useAuth } from "@/main";
import { createFileRoute, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard")({
  component: RouteComponent,
});

function RouteComponent() {
  const { isAuth } = useAuth();

  return (
    <PrivateRoute isAllowed={isAuth}>
      <Outlet />
    </PrivateRoute>
  );
}
