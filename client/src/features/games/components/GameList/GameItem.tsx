import { getImageUrl } from "@/utils/getImageUrl";
import { Link } from "@tanstack/react-router";
import type { Game } from "../../types/Game";

interface GameItemProps {
  game: Game;
}

export const GameItem = ({ game }: GameItemProps) => {
  return (
    <Link
      to="/games/$gameId"
      params={{ gameId: game.id }}
      className="flex cursor-pointer flex-row gap-x-4 items-center rounded-2xl hover:bg-gray-200"
    >
      <div className="rounded-2xl overflow-hidden w-28 h-28 flex-shrink-0">
        <img
          src={getImageUrl(game.thumbnailUrl)}
          alt="Thumbnail"
          className="object-contain w-full h-full"
        />
      </div>
      <div className="flex flex-col text-xs gap-y-1">
        <h2 className="text-gray-900 text-sm">{game.name}</h2>
        <p className="text-gray-700 h-8 overflow-clip">{game.description}</p>
        <p className="text-gray-700">
          <span className="text-gray-600">Price</span> ${game.price / 100}
        </p>
        <p className="text-gray-700">
          <span className="text-gray-600">Type</span> {game.locationType}
        </p>

        <data className="text-gray-600">
          {new Date(game.publishedAt).toLocaleDateString()}
        </data>
      </div>
    </Link>
  );
};
