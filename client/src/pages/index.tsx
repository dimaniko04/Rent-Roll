import { useAuth } from "@/main";
import { createFileRoute, Navigate } from "@tanstack/react-router";

export const Route = createFileRoute("/")({
  component: Index,
});

function Index() {
  const { isAuth } = useAuth();

  return <Navigate to={isAuth ? "/games" : "/login"} replace />;
}
