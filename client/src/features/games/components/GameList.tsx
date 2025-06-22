import { useSearch } from "@tanstack/react-router";
import type { GameFilters } from "../types/GameFilters";
import { useGames } from "../hooks/useGames";
import { useEffect, useRef } from "react";
import { GameItem } from "./GameItem";

export const GameList = () => {
  const scrollRef = useRef<HTMLDivElement>(null);
  const filters = useSearch({ strict: false }) as GameFilters;

  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isLoading,
    isError,
  } = useGames(filters);

  useEffect(() => {
    const element = scrollRef.current;
    if (!element) return;

    const onScroll = () => {
      const { scrollTop, scrollHeight, clientHeight } = element;
      if (scrollTop + clientHeight >= scrollHeight - 5 && hasNextPage) {
        fetchNextPage();
      }
    };

    element.addEventListener("scroll", onScroll);
    return () => {
      element.removeEventListener("scroll", onScroll);
    };
  });

  return (
    <div ref={scrollRef} className="overflow-y-auto px-6 pb-8">
      {isLoading && <div className="text-center py-4">Loading games...</div>}
      {isError && (
        <div className="text-center py-4 text-red-500">Error loading games</div>
      )}

      <ul className="grid auto-fit-min-max-500 gap-6">
        {data?.pages.map((page) =>
          page.items.map((game) => <GameItem key={game.id} game={game} />)
        )}
      </ul>

      {isFetchingNextPage && (
        <div className="text-center py-4">Loading more games...</div>
      )}
    </div>
  );
};
