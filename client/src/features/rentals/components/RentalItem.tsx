import { getImageUrl } from "@/utils/getImageUrl";
import type { Rental } from "../types/Rental";
import { useMutation } from "@tanstack/react-query";
import { RentalService } from "../services/RentalService";
import { AxiosError } from "axios";
import { toast } from "react-toastify";
import { Route } from "@/pages/rentals";

interface Props {
  rental: Rental;
}

export const RentalItem = ({ rental }: Props) => {
  const navigate = Route.useNavigate();
  let statusClasses = "";

  switch (rental.status) {
    case "Active":
      statusClasses = "bg-green-100 text-green-800";
      break;
    case "Expectation":
      statusClasses = "bg-gray-300 text-grey-700";
      break;
    case "Returned":
      statusClasses = "bg-gray-700 text-white-active";
      break;
    case "Overdue":
      statusClasses = "bg-red-100 text-danger-active";
      break;
  }

  const openCellMutatition = useMutation({
    mutationFn: async () => {
      return await RentalService.openCell(
        rental.id,
        rental.status === "Expectation"
      );
    },
    onError: (error: Error) => {
      if (error instanceof AxiosError) {
        const axiosError = error as AxiosError;

        if ("response" in axiosError) {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          const response = axiosError.response as any;

          toast.error(
            `Error opening the cell: ${response?.status} ${response.data.message}`
          );
          return;
        }
      }

      toast.error(`Error opening the cell: ${error.message}`);
    },
    onSuccess: () => {
      toast.success("Cell opened successfully");
      navigate({ reloadDocument: true, to: "/rentals" });
    },
  });

  return (
    <div key={rental.id} className="border-b pb-6 mb-6">
      <div className="flex gap-4">
        <img
          src={getImageUrl(rental.gameThumbnail)}
          alt="Thumbnail"
          className="w-24 h-28 object-cover rounded"
        />
        <div className="flex-1">
          <h3 className="text-lg font-semibold">{rental.gameName}</h3>
          <p className="text-sm text-gray-500">Price</p>
          <p className="text-base font-medium">
            ${(rental.totalPrice / 100).toFixed(2)}
          </p>
        </div>
      </div>

      <div className="mt-3">
        <span
          className={`${statusClasses} text-sm px-3 py-1 rounded-full inline-block mb-2`}
        >
          {rental.status}
        </span>

        <div className="flex gap-8 text-sm text-gray-700 mb-1">
          <div>
            <p className="font-medium text-gray-600">Start</p>
            <p>{new Date(rental.startDate).toLocaleDateString()}</p>
          </div>
          <div>
            <p className="font-medium text-gray-600">End</p>
            <p>{new Date(rental.endDate).toLocaleDateString()}</p>
          </div>
        </div>

        <p className="text-sm text-gray-700">{rental.locationName}</p>
        <p className="text-sm">
          <span className="font-medium">Address</span>
        </p>
        <p className="text-sm text-gray-700">{rental.address}</p>
      </div>

      {rental.iotDeviceId && (
        <button
          disabled={rental.status === "Returned" || rental.status === "Overdue"}
          onClick={() => openCellMutatition.mutate()}
          className={`cursor-pointer mt-4 w-32 bg-blue-600 hover:bg-blue-700 disabled:bg-blue-300 text-white py-2 px-4 font-medium rounded ${rental.status === "Active" ? "bg-purple-700 hover:bg-purple-500" : ""}`}
        >
          {rental.status !== "Active" && "Open Cell"}
          {rental.status === "Active" && "Return"}
        </button>
      )}
    </div>
  );
};
