import type { GameDetails } from "../../types/GameDetails";
import { RentalForm } from "./RentalForm";

interface Props {
  isOpen: boolean;
  game: GameDetails;
  onClose: () => void;
}

export const RentalSidePanel = ({ isOpen, onClose, game }: Props) => {
  return (
    <div
      className={`fixed h-full w-80 bg-white right-0 top-0 shadow p-6 ${isOpen ? "translate-x-0" : "translate-x-full"}`}
    >
      <div className="flex justify-between items-center mb-8">
        <p className="text-sm uppercase text-gray-700">Rent</p>
        <button
          onClick={onClose}
          className="text-gray-500 text-3xl cursor-pointer hover:text-gray-700"
        >
          <span className="p-2">&times;</span>
        </button>
      </div>

      <div className="flex flex-col gap-4">
        <div>
          <p className="text-gray-600 text-sm">
            Type
            <span className="ml-2 text-gray-700">{game.locationType}</span>
          </p>
        </div>
        <div>
          <div className="mb-1 text-gray-600 text-sm">Name</div>
          <p className="text-gray-700">{game.locationName}</p>
        </div>

        <div>
          <div className="mb-1 text-gray-600 text-sm">Address</div>
          <p className="text-gray-700">{game.locationAddress}</p>
        </div>
        <RentalForm
          prices={game.prices}
          id={game.id}
          type={game.locationType}
          onClose={onClose}
        />
      </div>
    </div>
  );
};
