import { Route } from "@/pages/rentals";
import { useRouter } from "@tanstack/react-router";
import { RentalItem } from "./RentalItem";

export const RentalsList = () => {
  const route = useRouter();
  const rentals = Route.useLoaderData();

  return (
    <div className="mx-auto px-6 py-4">
      <div className="mb-6 flex flex-row items-center">
        <div>
          <button
            onClick={() => route.history.back()}
            className="p-4 inline cursor-pointer text-gray-700 group"
          >
            <span className="inline-block mr-2 transition group-hover:-translate-x-2.5">
              ←
            </span>
            Back
          </button>
        </div>

        <h2 className="text-4xl grow-1 text-center font-bold">Rentals</h2>
      </div>

      <div className="grid auto-fit-min-max-500 gap-x-8 gap-y-3">
        {rentals.map((rental) => (
          <RentalItem rental={rental} key={rental.id} />
        ))}
      </div>
    </div>
  );
};
