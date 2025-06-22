import { useQuery } from "@tanstack/react-query";
import axios from "axios";

type Country = {
  name: {
    common: string;
  };
};

export const useCountry = (search: string) => {
  return useQuery<string[]>({
    queryKey: ["country", search],
    queryFn: async () => {
      const { data } = await axios.get<Country[]>(
        `https://restcountries.com/v3.1/name/${encodeURIComponent(search)}`
      );

      return data.map((country) => country.name.common);
    },
    enabled: !!search,
    staleTime: 5 * 60 * 1000,
  });
};
