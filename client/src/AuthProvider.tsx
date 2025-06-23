import { useState, useEffect, type PropsWithChildren } from "react";
import { AuthService } from "./features/auth";
import { AuthContext } from "./main";

export const AuthProvider = ({ children }: PropsWithChildren) => {
  const [isAuth, setIsAuth] = useState(false);

  useEffect(() => {
    const isAuthenticated = async () => {
      try {
        const response = await AuthService.refresh();
        localStorage.setItem("token", response.data.accessToken);
        setIsAuth(true);
      } catch (error) {
        console.error("Authentication check failed:", error);
        localStorage.removeItem("token");
        setIsAuth(false);
      }
    };

    isAuthenticated();
  }, []);

  return <AuthContext value={{ isAuth, setIsAuth }}>{children}</AuthContext>;
};
