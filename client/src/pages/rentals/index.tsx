import { RentalsList } from "@/features/rentals/components/RentalsList";
import { RentalService } from "@/features/rentals/services/RentalService";
import { createFileRoute, Navigate } from "@tanstack/react-router";
import { AxiosError } from "axios";
import { toast } from "react-toastify";

export const Route = createFileRoute("/rentals/")({
  component: RouteComponent,
  loader: () => RentalService.getAll(),
  pendingComponent: () => <div>Loading rentals...</div>,
  errorComponent: ({ error }) => {
    if (error instanceof AxiosError) {
      const axiosError = error as AxiosError;

      if ("response" in axiosError) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const response = axiosError.response as any;

        toast.error(
          `Error loading rentals: ${response?.status} ${response.data.message}`
        );
        return <Navigate to="/games" replace />;
      }
    }

    toast.error(`Error loading rentals: ${error.message}`);
    return <Navigate to="/games" replace />;
  },
});

function RouteComponent() {
  return (
    <div>
      <RentalsList />
    </div>
  );
}
