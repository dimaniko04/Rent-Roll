import api from "@/services/api";
import type { AxiosResponse } from "axios";
import type { CreateRental } from "../types/CreateRental";

export class RentalService {
  static async create(createReq: CreateRental): Promise<AxiosResponse> {
    return api.post("rentals", createReq);
  }
}
