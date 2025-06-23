import api from "@/services/api";
import type { Game } from "../types/Game";
import type { GameFilters } from "../types/GameFilters";
import type { PaginatedList } from "@/types/PaginatedList";
import type { GameDetails } from "../types/GameDetails";

export class GameService {
  static async getGames(filters: GameFilters) {
    const response = await api.get<PaginatedList<Game>>("/games/rent", {
      params: filters,
      paramsSerializer: (params) => {
        const searchParams = new URLSearchParams();
        for (const [key, value] of Object.entries(params)) {
          if (Array.isArray(value)) {
            value.forEach((v) => searchParams.append(key, v));
          } else if (value !== undefined && value !== null) {
            searchParams.append(key, String(value));
          }
        }
        return searchParams.toString();
      },
    });
    return response.data;
  }

  static async getGameById(id: string) {
    const response = await api.get<GameDetails>(`/games/rent/${id}`);
    return response.data;
  }

  static async getGenres() {
    const response = await api.get("/genres");
    return response.data;
  }

  static async getCategories() {
    const response = await api.get("/categories");
    return response.data;
  }

  static async getMechanics() {
    const response = await api.get("/mechanics");
    return response.data;
  }
}
