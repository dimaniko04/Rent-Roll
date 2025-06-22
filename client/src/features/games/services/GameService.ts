import api from "@/services/api";

export class GameService {
  static async getGames(
    page: number = 1,
    pageSize: number = 10,
    search: string = "",
    genres: string[] = [],
    categories: string[] = [],
    mechanics: string[] = [],
    isVerified: boolean = true
  ) {
    const params: Record<string, string> = {
      page: page.toString(),
      pageSize: pageSize.toString(),
      search,
      genres: genres.join(","),
      categories: categories.join(","),
      mechanics: mechanics.join(","),
      isVerified: isVerified ? "true" : "false",
    };

    const response = await api.get(`/api/games?${new URLSearchParams(params)}`);
    return response.data;
  }

  static async getGenres() {
    const response = await api.get("/api/genres");
    return response.data;
  }

  static async getCategories() {
    const response = await api.get("/api/categories");
    return response.data;
  }

  static async getMechanics() {
    const response = await api.get("/api/mechanics");
    return response.data;
  }
}
