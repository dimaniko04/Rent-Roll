import { PasswordInput } from "@/components/PasswordInput";
import { TextInput } from "@/components/TextInput";
import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

const LoginFormSchema = z.object({
  email: z.string().nonempty("Required").email("Invalid email address"),
  password: z.string().nonempty("Required"),
});

interface LoginFormData {
  email: string;
  password: string;
}

export const LoginForm = () => {
  const { control, handleSubmit } = useForm<LoginFormData>({
    defaultValues: {
      email: "",
      password: "",
    },
    resolver: zodResolver(LoginFormSchema),
  });
  const [passwordVisible, setPasswordVisible] = useState(false);

  const toggleVisibility = () => {
    setPasswordVisible((prev) => !prev);
  };

  const onSubmit = (data: LoginFormData) => {
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

      <button type="submit" className="btn-primary">
        Login
      </button>
    </form>
  );
};
