import api from "@/services/api";
import { jwtDecode } from "jwt-decode";
import type { Game } from "../types/Game";
import type { GameFilters } from "../types/GameFilters";
import type { PaginatedList } from "@/types/PaginatedList";
import type { GameDetails } from "../types/GameDetails";

const decodeCountry = (token: string | null) => {
  if (!token) return null;

  const decoded = jwtDecode(token) as Record<string, string>;
  const key = Object.keys(decoded).find((key) => key.includes("country"));

  if (!key) return null;

  return decoded[key] as string;
};

export class GameService {
  static async getGames(filters: GameFilters) {
    const token = localStorage.getItem("token");
    const country = decodeCountry(token);

    const response = await api.get<PaginatedList<Game>>("/games/rent", {
      params: { ...filters, country: country },
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
