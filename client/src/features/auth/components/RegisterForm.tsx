import { PasswordInput } from "@/components/PasswordInput";
import { TextInput } from "@/components/TextInput";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { CountrySelect } from "./CountrySelect";
import { useMutation } from "@tanstack/react-query";
import { AuthService } from "../services/AuthService";
import { AxiosError } from "axios";
import { handleAxiosError } from "@/utils/handleAxiosError";
import { useAuth } from "@/main";
import { useNavigate } from "@tanstack/react-router";

const RegisterFormSchema = z
  .object({
    email: z.string().nonempty("Required").email("Invalid email address"),
    password: z
      .string()
      .nonempty("Required")
      .min(8, "Password must be at least 8 characters long"),
    confirmPassword: z.string().nonempty("Required"),
    fullName: z.string().nonempty("Required"),
    country: z.string().nonempty("Required"),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
  });

interface RegisterFormData {
  email: string;
  password: string;
  fullName: string;
  country: string;
  confirmPassword: string;
}

export const RegisterForm = () => {
  const { control, handleSubmit, setError } = useForm<RegisterFormData>({
    defaultValues: {
      email: "",
      password: "",
      fullName: "",
      country: "",
      confirmPassword: "",
    },
    mode: "onBlur",
    resolver: zodResolver(RegisterFormSchema),
  });
  const { setIsAuth } = useAuth();
  const navigate = useNavigate();
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [serverError, setServerError] = useState<string | null>(null);

  const toggleVisibility = () => {
    setPasswordVisible((prev) => !prev);
  };

  const registerMutation = useMutation({
    mutationFn: async (values: RegisterFormData) => {
      return await AuthService.register(
        values.email,
        values.password,
        values.fullName,
        values.country
      );
    },
    onError: (error: Error) => {
      if (error instanceof AxiosError) {
        const message = handleAxiosError(error);

        if (typeof message === "string") {
          setServerError(message);
        } else if (typeof message === "object") {
          Object.entries(message).forEach(([field, messages]) => {
            messages.forEach((msg) => {
              setError(field as keyof RegisterFormData, {
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

  const onSubmit = (data: RegisterFormData) => {
    registerMutation.mutate(data);
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

      <PasswordInput
        isVisible={passwordVisible}
        toggleVisibility={toggleVisibility}
        label="Confirm Password"
        control={control}
        name="confirmPassword"
      />

      <TextInput label="Full Name" control={control} name="fullName" />

      <CountrySelect label="Country" name="country" control={control} />

      {serverError && (
        <div className="text-red-500 text-sm mb-2">{serverError}</div>
      )}

      <button
        type="submit"
        className="btn-primary disabled:bg-primary-light"
        disabled={registerMutation.isPending}
      >
        Sign In
      </button>
    </form>
  );
};
