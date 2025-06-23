import { useState } from "react";
import type { UseQueryResult } from "@tanstack/react-query";

import type { GameFilters } from "../../types/GameFilters";
import { AsyncFilterDropDown } from "../AsyncFilterDropDown";
import { useGameFilters } from "../hooks/useGameFilters";

type WithName = {
  name: string;
};

interface Props {
  icon: string;
  title: string;
  name: keyof GameFilters;
  query: (enabled: boolean) => UseQueryResult<WithName[], Error>;
}

export const GameAsyncFilter = ({ title, icon, name, query }: Props) => {
  const { search, updateSearch } = useGameFilters();
  const [isTriggered, setIsTriggered] = useState(false);

  const { data, isLoading, error } = query(isTriggered);

  return (
    <AsyncFilterDropDown
      title={title}
      icon={icon}
      isLoading={isLoading}
      error={error?.message}
      selected={(search[name] as string[]) ?? []}
      options={data?.map(({ name }) => ({ label: name, value: name }))}
      onOpen={() => setIsTriggered(true)}
      onSelect={(values) =>
        updateSearch({ [name]: values.length > 0 ? values : undefined })
      }
    />
  );
};
