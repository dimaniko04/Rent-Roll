export type Rental = {
  id: string;
  status: string;
  startDate: Date;
  endDate: Date;
  totalPrice: number;
  address: string;
  gameName: string;
  gameThumbnail: string;
  locationName: string;
  iotDeviceId: string | null;
};
