import { PasswordInput } from "@/components/PasswordInput";
import { TextInput } from "@/components/TextInput";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { AxiosError } from "axios";
import { AuthService } from "../services/AuthService";
import { handleAxiosError } from "@/utils/handleAxiosError";
import { useNavigate } from "@tanstack/react-router";
import { useAuth } from "@/main";

const LoginFormSchema = z.object({
  email: z.string().nonempty("Required").email("Invalid email address"),
  password: z.string().nonempty("Required"),
});

interface LoginFormData {
  email: string;
  password: string;
}

export const LoginForm = () => {
  const { control, handleSubmit, setError } = useForm<LoginFormData>({
    defaultValues: {
      email: "",
      password: "",
    },
    mode: "onBlur",
    resolver: zodResolver(LoginFormSchema),
  });
  const { setIsAuth } = useAuth();
  const navigate = useNavigate();
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [serverError, setServerError] = useState<string | null>(null);

  const toggleVisibility = () => {
    setPasswordVisible((prev) => !prev);
  };

  const loginMutation = useMutation({
    mutationFn: async (values: LoginFormData) => {
      return await AuthService.login(values.email, values.password);
    },
    onError: (error: Error) => {
      if (error instanceof AxiosError) {
        const message = handleAxiosError(error);

        if (typeof message === "string") {
          setServerError(message);
        } else if (typeof message === "object") {
          Object.entries(message).forEach(([field, messages]) => {
            messages.forEach((msg) => {
              setError(field as keyof LoginFormData, {
                type: "validate",
                message: msg,
              });
            });
          });
        }
      } else {
        console.error("Unexpected error:", error);
        setServerError("An unexpected error occurred.");
      }
    },
    onSuccess: (response) => {
      setServerError(null);
      setIsAuth(true);
      localStorage.setItem("token", response.data.accessToken);
      console.log("Logged in:", response);
      navigate({ to: "/games" });
    },
  });

  const onSubmit = (data: LoginFormData) => {
    loginMutation.mutate(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-y-4">
      <TextInput type="email" label="Email" control={control} name="email" />

      <PasswordInput
        isVisible={passwordVisible}
        toggleVisibility={toggleVisibility}
        label="Password"
        control={control}
        name="password"
      />

      {serverError && (
        <div className="text-red-500 text-sm mb-2">{serverError}</div>
      )}

      <button
        type="submit"
        className="btn-primary disabled:bg-primary-light"
        disabled={loginMutation.isPending}
      >
        Login
      </button>
    </form>
  );
};
