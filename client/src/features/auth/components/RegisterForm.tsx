import { PasswordInput } from "@/components/PasswordInput";
import { TextInput } from "@/components/TextInput";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { CountrySelect } from "./CountrySelect";

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
  const { control, handleSubmit } = useForm<RegisterFormData>({
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
  const [passwordVisible, setPasswordVisible] = useState(false);

  const toggleVisibility = () => {
    setPasswordVisible((prev) => !prev);
  };

  const onSubmit = (data: RegisterFormData) => {
    console.log("Form submitted with data:", data);
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

      <button type="submit" className="btn-primary">
        Sign In
      </button>
    </form>
  );
};
