import { AuthService } from "@/features/auth";
import { useAuth } from "@/main";
import { createFileRoute, useNavigate } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard/games")({
  component: RouteComponent,
});

function RouteComponent() {
  const { setIsAuth } = useAuth();
  const navigation = useNavigate();

  const logout = async () => {
    await AuthService.logout();
    setIsAuth(false);
    localStorage.removeItem("token");
    navigation({ to: "/login" });
  };

  return (
    <div>
      <button onClick={logout}>Logout</button>
    </div>
  );
}
