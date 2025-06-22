import { createFileRoute, Outlet } from "@tanstack/react-router";
import background from "@assets/img/background.png";
import logo from "@assets/icons/logo.svg";
import logoText from "@assets/icons/logoText.svg";

export const Route = createFileRoute("/_auth")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="flex h-full flex-row">
      <div className="flex-3/5">
        <img src={background} alt="cover" />
      </div>
      <div className="flex-2/5 flex flex-col items-center mt-20 mb-6 gap-y-12">
        <div>
          <img src={logo} alt="Logo" className="inline mr-2" />
          <img src={logoText} alt="Diploma" className="inline" />
        </div>

        <Outlet />
      </div>
    </div>
  );
}
