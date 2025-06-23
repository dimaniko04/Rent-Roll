import { Route } from "@/pages/_dashboard/games";
import type { GameFilters } from "../../types/GameFilters";

export const useGameFilters = () => {
  const navigate = Route.useNavigate();
  const search = Route.useSearch() as GameFilters;

  const updateSearch = (updates: Partial<GameFilters>) => {
    navigate({
      search: {
        ...search,
        ...updates,
      },
      replace: true,
    });
  };

  return {
    search,
    updateSearch,
  };
};
