import companyLogo from "@assets/dice-logo.svg";
import { createLazyFileRoute, Link } from "@tanstack/react-router";

export const Route = createLazyFileRoute("/register")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div>
      <div>
        <img
          src={companyLogo}
          className="mx-auto my-[-20px]"
          alt="Logo"
          width={196}
          height={196}
        />
        <h2 className="font-dynapuff text-5xl text-center">Rent&Roll</h2>
      </div>
      <h1 className="text-center">Register (TODO)</h1>
      <Link to="/login">Already have an account?</Link>
    </div>
  );
}
