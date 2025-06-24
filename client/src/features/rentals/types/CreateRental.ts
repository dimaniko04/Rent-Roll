export type CreateRental = {
  id: string;
  unit: "hour" | "day" | "week" | "month";
  term: number;
  type: string;
};
