import api from "@/services/api";
import type { AuthResponse } from "@/types/AuthResponse";
import type { AxiosResponse } from "axios";

export class AuthService {
  static async login(
    email: string,
    password: string
  ): Promise<AxiosResponse<AuthResponse>> {
    return api.post<AuthResponse>("authentication/login", { email, password });
  }

  static async register(
    email: string,
    password: string,
    fullName: string,
    country: string
  ): Promise<AxiosResponse<AuthResponse>> {
    return api.post<AuthResponse>("authentication/registration", {
      email,
      password,
      fullName,
      country,
    });
  }

  static async logout(): Promise<void> {
    return await api.post("authentication/logout");
  }

  static async refresh(): Promise<AxiosResponse<AuthResponse>> {
    return await api.post<AuthResponse>("authentication/refresh");
  }
}
