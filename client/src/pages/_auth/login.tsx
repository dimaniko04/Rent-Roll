import { createFileRoute, Link } from "@tanstack/react-router";

import { LoginForm } from "@features/auth";

export const Route = createFileRoute("/_auth/login")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div>
        <h1 className="font-medium text-2xl leading-9 mb-2.5 text-gray-900">
          Welcome to Rent&Roll
        </h1>
        <p className="text-gray-700 text-sm leading-5">
          Please sign-in to your account
        </p>
      </div>

      <LoginForm />

      <Link
        to="/register"
        className="mt-auto py-3.5 text-sm leading-5 text-gray-600 group"
      >
        <p>
          New on out platform
          <span className="ml-2.5 text-primary group-hover:underline">
            Create an account
          </span>
        </p>
      </Link>
    </>
  );
}
