import { RegisterForm } from "@/features/auth";
import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/_auth/register")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div className="w-max">
        <h1 className="font-medium text-2xl leading-9 mb-2.5 text-gray-900">
          Create your account
        </h1>
        <p className="text-gray-700 text-sm leading-5">
          Register to access the platform
        </p>
      </div>

      <RegisterForm />

      <Link
        to="/login"
        className="mt-auto py-3.5 text-sm leading-5 text-gray-600 group"
      >
        <p>
          Already have an account?
          <span className="ml-2.5 text-primary group-hover:underline">
            Sign in instead
          </span>
        </p>
      </Link>
    </>
  );
}
