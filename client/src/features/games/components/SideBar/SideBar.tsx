import logo from "@assets/icons/logo.svg";
import logoText from "@assets/icons/logoText.svg";
import rent from "@assets/icons/rent.svg";
import genre from "@assets/icons/genre.svg";
import category from "@assets/icons/category.svg";
import mechanic from "@assets/icons/mechanic.svg";

import { GameAsyncFilter } from "./GameAsyncFilter";
import { useCategories, useGenres, useMechanics } from "../../hooks/useGames";
import { useGameFilters } from "../hooks/useGameFilters";
import { RadioDropDown } from "../RadioDropDown";
import type { GameFilters } from "../../types/GameFilters";

export const SideBar = () => {
  const { search, updateSearch } = useGameFilters();

  return (
    <div className="p-6 flex flex-col">
      <div className="flex flex-row items-center gap-x-2 mb-6">
        <img src={logo} alt="" />
        <img src={logoText} alt="" />
      </div>

      <div className="flex flex-col gap-y-2">
        <GameAsyncFilter
          name="genres"
          title="Genre"
          icon={genre}
          query={useGenres}
        />
        <GameAsyncFilter
          name="categories"
          title="Category"
          icon={category}
          query={useCategories}
        />
        <GameAsyncFilter
          name="mechanics"
          title="Mechanic"
          icon={mechanic}
          query={useMechanics}
        />

        <RadioDropDown<GameFilters["timeUnit"]>
          title="Rent"
          icon={rent}
          options={[
            { label: "Hourly", value: "hour" },
            { label: "Daily", value: "day" },
            { label: "Weekly", value: "week" },
            { label: "Monthly", value: "month" },
          ]}
          selected={search.timeUnit}
          onSelect={(value) => updateSearch({ timeUnit: value })}
        />

        <label className="flex items-center w-full px-4 py-2 text-sm text-gray-900">
          Verification
          <input
            type="radio"
            checked={search.isVerified ?? false}
            onClick={() =>
              updateSearch({ isVerified: !search.isVerified || undefined })
            }
            className="radio ml-4 w-4 h-4"
          />
        </label>
      </div>
    </div>
  );
};
