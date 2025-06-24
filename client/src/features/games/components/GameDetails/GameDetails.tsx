import { Route } from "@/pages/_dashboard/games.$gameId";
import { Link } from "@tanstack/react-router";

import verifiedIcon from "@assets/icons/verified.svg";
import notVerifiedIcon from "@assets/icons/notVerified.svg";
import { getImageUrl } from "@/utils/getImageUrl";
import { useState } from "react";
import { RentalSidePanel } from "./RentalSidePanel";

export const GameDetails = () => {
  const game = Route.useLoaderData();
  const [panelOpen, setPanelOpen] = useState(false);

  return (
    <>
      <RentalSidePanel
        game={game}
        isOpen={panelOpen}
        onClose={() => setPanelOpen(false)}
      />

      <div className="px-6 pb-8 overflow-y-auto flex flex-col gap-6">
        <nav style={{ marginBottom: 16 }}>
          <p className="text-xs font-medium text-gray-500">
            <Link to="/games" className="p-2">
              Home
            </Link>
            /<span className="text-primary-active p-2">Details game</span>
          </p>
        </nav>

        <div className="py-5 gap-8 flex flex-row items-center">
          <img
            src="https://cf.geekdo-images.com/x3zxjr-Vw5iU4yDPg70Jgw__itemrep/img/giNUMut4HAl-zWyQkGG0YchmuLI=/fit-in/246x300/filters:strip_icc()/pic3490053.jpg"
            alt="Game Thumbnail"
            className="w-32 rounded overflow-hidden"
          />

          <div className="flex-1 space-y-2">
            <h1 className="text-lg font-medium">{game.name}</h1>
            <img src={game.isVerified ? verifiedIcon : notVerifiedIcon} />

            <div className="pt-2 text-xs text-gray-600">
              <p className="pb-1">Type: {game.locationType}</p>
              <p>{new Date(game.publishedAt).toLocaleDateString()}</p>
            </div>
          </div>

          <button
            onClick={() => setPanelOpen(true)}
            className="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded font-medium"
          >
            Rent ${game.prices[0].price / 100}
          </button>
        </div>

        <div className="gap-2 flex flex-col">
          <h2 className="text-sm uppercase font-medium text-gray-700">
            IMAGE GALLERY
          </h2>
          <div className=" flex flex-row overflow-x-scroll gap-2 -mr-6">
            {game.images.length === 0 && (
              <p className="text-center m-auto w-full">No images found!</p>
            )}
            {game.images.map((image, index) => (
              <img
                src={getImageUrl(image)}
                about={`Image ${index + 1}`}
                className="h-36 w-68 shrink-0 rounded-md overflow-hidden object-cover object-center"
              />
            ))}
          </div>

          <p className="text-gray-600 text-sm">{game.description}</p>
        </div>

        <div>
          <h2 className="text-sm uppercase mb-2 font-medium text-gray-700">
            INFORMATION
          </h2>
          <div className="flex flex-col gap-4 *:gap-x-16">
            <div className="flex flew-row text-sm *:*:first-of-type:text-gray-600 *:*:first-of-type:mb-2 *:*:text-gray-700">
              <div>
                <div>Min players</div>
                <div>{game.minPlayers}</div>
              </div>
              <div>
                <div>Age Rating</div>
                <div>{game.age}+</div>
              </div>
              <div>
                <div>Average Playtime</div>
                <div>
                  {game.averagePlayTime
                    ? game.averagePlayTime + " min"
                    : "no data"}
                </div>
              </div>
            </div>

            <div className="flex flew-row text-sm *:*:first-of-type:text-gray-600 *:*:first-of-type:mb-2 *:*:text-gray-700">
              <div>
                <div>Max players</div>
                <div>{game.maxPlayers}</div>
              </div>
              <div>
                <div>Complexity</div>
                <div>
                  <span className="text-blue-400">
                    {game.complexityScore
                      ? (game.complexityScore / 100).toFixed(2)
                      : "?"}
                  </span>
                  /5
                </div>
              </div>
            </div>

            <div className="flex flex-row  text-sm *:*:first-of-type:text-gray-600 *:*:text-primary-active *:*:first-of-type:mb-2 *:grow *:shrink flex-wrap">
              <div>
                <div>Genres</div>
                <div>{game.genres.join(", ")}</div>
              </div>
              <div>
                <div>Categories</div>
                <div>{game.categories.join(", ")}</div>
              </div>
              <div>
                <div>Mechanics</div>
                <div>{game.mechanics.join(", ")}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
