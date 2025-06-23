export type GameDetails = {
  id: string;
  businessGameId: string;
  name: string;
  description: string;
  thumbnailUrl?: string;
  publishedAt: Date;
  minPlayers: number;
  maxPlayers: number;
  age: number;
  averagePlayTime?: number;
  complexityScore?: number;
  isVerified: boolean;
  genres: string[];
  categories: string[];
  mechanics: string[];
  images: string[];
  prices: GamePrice[];
  locationName: string;
  locationType: string;
  locationAddress: string;
};

export type GamePrice = {
  price: number;
  unitCount: number;
  timeUnit: "hour" | "day" | "week" | "month";
};
