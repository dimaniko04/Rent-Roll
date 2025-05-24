import { zodResolver } from "@hookform/resolvers/zod";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useForm, type FieldValues } from "react-hook-form";
import { z } from "zod";

const loginSchema = z.object({
  email: z
    .string()
    .nonempty("Email is required")
    .email("Invalid email address"),
  password: z.string().nonempty("Password is required"),
});

type TLoginSchema = z.infer<typeof loginSchema>;

export const Route = createLazyFileRoute("/login")({
  component: RouteComponent,
});

function RouteComponent() {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<TLoginSchema>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: FieldValues) => {
    await new Promise((resolve) => setTimeout(resolve, 2000));
    console.log("Form submitted", data);
    reset();
  };

  return (
    <form
      className="h-full flex flex-col items-center justify-center"
      onSubmit={handleSubmit(onSubmit)}
    >
      <input
        className="mb-3 p-2 rounded-md border border-gray-400"
        {...register("email")}
        type="email"
        placeholder="Email"
      />
      {errors.email && <p>{`${errors.email.message}`}</p>}

      <input
        className="mb-3 p-2 rounded-md border border-gray-400"
        {...register("password")}
        type="password"
        placeholder="Password"
      />
      {errors.password && <p>{`${errors.password.message}`}</p>}

      <button
        className="w-[200px] cursor-pointer bg-purple-600 text-white rounded-xl px-4 py-2 hover:bg-purple-700 transition-colors duration-300"
        type="submit"
        disabled={isSubmitting}
      >
        {isSubmitting ? "Loading..." : "Login"}
      </button>
    </form>
  );
}
