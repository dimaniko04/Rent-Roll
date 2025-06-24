import api from "@/services/api";
import type { CreateRental } from "../types/CreateRental";
import type { Rental } from "../types/Rental";

export class RentalService {
  static async create(createReq: CreateRental) {
    return api.post("rentals", createReq);
  }

  static async getAll() {
    const res = await api.get<Rental[]>("rentals/my");
    return res.data;
  }

  static async openCell(id: string, isPickUp: boolean) {
    return api.put<Rental>(
      `rentals/${id}/open/${isPickUp ? "PickUp" : "Return"}`
    );
  }
}
