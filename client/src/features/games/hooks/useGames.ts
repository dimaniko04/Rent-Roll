import { useInfiniteQuery } from "@tanstack/react-query";
import { GameService } from "../services/GameService";
import type { GameFilters } from "../types/GameFilters";

import { type Game } from "../types/Game";
import type { PaginatedList } from "@/types/PaginatedList";

export const useGames = (filters: GameFilters) => {
  return useInfiniteQuery<PaginatedList<Game>>({
    queryKey: ["games", filters],
    initialPageParam: 1,
    queryFn: async () => await GameService.getGames(filters),
    getNextPageParam: (lastPage) => {
      if (lastPage.hasNext) {
        return lastPage.currentPage + 1;
      }
      return undefined;
    },
    staleTime: 5 * 60 * 1000,
  });
};
