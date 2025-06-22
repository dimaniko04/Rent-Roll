import { Navigate, Outlet } from "@tanstack/react-router";
import { type PropsWithChildren } from "react";

interface Props {
  isAllowed: boolean;
  redirect?: string;
}

export const PrivateRoute = ({
  isAllowed,
  redirect = "/login",
  children,
}: PropsWithChildren<Props>) => {
  if (!isAllowed) {
    return <Navigate to={redirect} replace />;
  }

  return children || <Outlet />;
};
