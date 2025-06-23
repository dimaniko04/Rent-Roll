import { useInfiniteQuery, useQuery } from "@tanstack/react-query";
import { GameService } from "../services/GameService";
import type { GameFilters } from "../types/GameFilters";

import {
  type Game,
  type Genre,
  type Mechanic,
  type Category,
} from "../types/Game";
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

export const useGenres = (enabled: boolean) => {
  return useQuery<Genre[]>({
    queryKey: ["genres"],
    queryFn: async () => {
      return await GameService.getGenres();
    },
    enabled: enabled,
    staleTime: Infinity,
  });
};

export const useCategories = (enabled: boolean) => {
  return useQuery<Category[]>({
    queryKey: ["categories"],
    queryFn: async () => {
      return await GameService.getCategories();
    },
    enabled: enabled,
    staleTime: Infinity,
  });
};

export const useMechanics = (enabled: boolean) => {
  return useQuery<Mechanic[]>({
    queryKey: ["mechanic"],
    queryFn: async () => {
      return await GameService.getMechanics();
    },
    enabled: enabled,
    staleTime: Infinity,
  });
};
