import { useEffect, useMemo, useState } from "react";
import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import debounce from "lodash/debounce";
import Select from "react-select";

import { useCountry } from "../hooks/useCountry";

interface Props<T extends FieldValues> {
  label: string;
  name: Path<T>;
  control: Control<T>;
}

export const CountrySelect = <T extends FieldValues>({
  label,
  name,
  control,
}: Props<T>) => {
  const fieldState = control.getFieldState(name);
  const [search, setSearch] = useState("");

  const { data: countries, isFetching, refetch } = useCountry(search);

  const debouncedSearch = useMemo(
    () =>
      debounce((value: string) => {
        setSearch(value);
      }, 500),
    []
  );

  useEffect(() => {
    if (search.length > 1) {
      refetch();
    }
  }, [search, refetch]);

  useEffect(() => {
    return () => {
      debouncedSearch.cancel();
    };
  }, [debouncedSearch]);

  return (
    <div>
      <label htmlFor={name} className="text-gray-600 text-sm leading-5">
        {label}
      </label>
      <div>
        <Controller
          name={name}
          control={control}
          render={({ field }) => (
            <Select
              styles={{
                control: (base) => ({
                  ...base,
                  "border border-gray-400 py-2 px-2.5 rounded-sm text-sm w-full":
                    true,
                }),
              }}
              onInputChange={debouncedSearch}
              isLoading={isFetching}
              options={
                countries?.map((country) => ({
                  label: country,
                  value: country,
                })) || []
              }
              placeholder="Search for a country"
              onBlur={field.onBlur}
              onChange={(option) => field.onChange(option?.value)}
              noOptionsMessage={() =>
                search.length < 2
                  ? "Start typing..."
                  : isFetching
                    ? "Searching..."
                    : "No countries found"
              }
            />
          )}
        />

        <span
          className={`text-danger float-end text-xs ${fieldState.error ? "visible" : "invisible"}`}
        >
          {fieldState.error?.message || ""}
        </span>
      </div>
    </div>
  );
};
