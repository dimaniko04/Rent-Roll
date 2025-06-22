import { createFileRoute, Navigate, Outlet } from "@tanstack/react-router";
import background from "@assets/img/background.png";
import logo from "@assets/icons/logo.svg";
import logoText from "@assets/icons/logoText.svg";
import { useAuth } from "@/main";

export const Route = createFileRoute("/_auth")({
  component: RouteComponent,
});

function RouteComponent() {
  const { isAuth } = useAuth();

  if (isAuth) {
    return <Navigate to="/games" replace />;
  }

  return (
    <div className="flex h-full flex-row overflow-hidden">
      <div className="flex-3/5">
        <img src={background} alt="cover" />
      </div>
      <div className="overflow-auto flex-2/5">
        <div className="flex flex-col items-center my-6">
          <div className="w-fit flex flex-col gap-y-12 h-full">
            <div>
              <img src={logo} alt="Logo" className="inline mr-2" />
              <img src={logoText} alt="Diploma" className="inline" />
            </div>

            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
}
