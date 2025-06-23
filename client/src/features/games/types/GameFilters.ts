export type GameFilters = {
  search?: string;
  locationType?: "gameStore" | "locker";
  timeUnit?: "hour" | "day" | "week" | "month";
  isVerified?: boolean;
  minAge?: number;
  maxAge?: number;
  genres?: string[];
  mechanics?: string[];
  categories?: string[];
  page?: number;
  pageSize?: number;
};
