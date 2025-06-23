import { Link } from "@tanstack/react-router";
import { debounce } from "lodash";
import { useEffect, useMemo, useState } from "react";

import { useAuth } from "@/main";
import profile from "@assets/icons/profile.svg";
import { Route } from "@/pages/_dashboard/games";

export const GamesHeader = () => {
  const { setIsAuth } = useAuth();
  const [isOpen, setIsOpen] = useState(false);
  const navigate = Route.useNavigate();

  const debouncedSearch = useMemo(
    () =>
      debounce((value: string) => {
        navigate({
          search: (prev) => ({ ...prev, search: value }),
          replace: true,
        });
      }, 500),
    [navigate]
  );

  useEffect(() => {
    return () => {
      debouncedSearch.cancel();
    };
  }, [debouncedSearch]);

  return (
    <div className="flex items-center p-6 gap-x-6 mb-2">
      <input
        className="rounded-xl border border-gray-400 py-2 px-2.5 text-sm flex-1"
        placeholder="Search"
        type="search"
        onChange={(e) => debouncedSearch(e.target.value)}
      />

      <div className="flex-1 flex justify-end gap-x-8">
        <Link to="/rentals">Rentals</Link>

        <div className="relative">
          <button onClick={() => setIsOpen(!isOpen)}>
            <img src={profile} alt="Profile" />
          </button>
          <button
            onClick={() => {
              setIsOpen(!isOpen);
              setIsAuth(false);
              localStorage.removeItem("token");
              navigate({ to: "/login" });
            }}
            className={`absolute cursor-pointer top-5 right-2 rounded-full ${isOpen ? "block" : "hidden"} bg-gray-300 shadow-lg p-4 hover:bg-gray-400`}
          >
            Logout
          </button>
        </div>
      </div>
    </div>
  );
};
