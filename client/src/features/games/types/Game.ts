export type Game = {
  id: string;
  gameId: string;
  businessGameId: string;
  locationId: string;
  locationType: "gameStore" | "locker";
  name: string;
  description: string;
  thumbnailUrl: string;
  publishedAt: Date;
  isVerified: boolean;
  price: number;
  address: string;
  timeUnit: "hour" | "day" | "week" | "month";
};

export type Genre = {
  id: string;
  name: string;
};

export type Category = {
  id: string;
  name: string;
};

export type Mechanic = {
  id: string;
  name: string;
};
