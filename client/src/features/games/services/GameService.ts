import api from "@/services/api";
import type { Game } from "../types/Game";
import type { GameFilters } from "../types/GameFilters";
import type { PaginatedList } from "@/types/PaginatedList";

export class GameService {
  static async getGames(filters: GameFilters) {
    const response = await api.get<PaginatedList<Game>>("/games/rent", {
      params: filters,
    });
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
