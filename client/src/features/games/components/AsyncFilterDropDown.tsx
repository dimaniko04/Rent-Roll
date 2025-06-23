import { useState } from "react";

import chevronDown from "@assets/icons/chevronDown.svg";

interface Props<T> {
  title: string;
  icon: string;
  options?: {
    value: T;
    label: string;
  }[];
  error?: string;
  isLoading: boolean;
  selected: T[];
  onOpen: () => void;
  onSelect: (value: T[]) => void;
}

export const AsyncFilterDropDown = <T,>(props: Props<T>) => {
  const {
    title,
    icon,
    options,
    error,
    isLoading = false,
    selected = [],
    onOpen,
    onSelect,
  } = props;

  const [open, setOpen] = useState(false);

  const toggleValue = (value: T) => {
    const newSelected = [...selected];
    const index = newSelected.findIndex((v) => v === value);

    if (index !== -1) newSelected.splice(index, 1);
    else newSelected.push(value);

    onSelect(newSelected);
  };

  const handleOpen = () => {
    setOpen(!open);
    if (!open) {
      onOpen();
    }
  };

  return (
    <div className="flex flex-col w-full">
      <button
        onClick={() => handleOpen()}
        className={`flex  cursor-pointer rounded-lg w-full px-4 py-2 ${open ? "bg-gray-300" : ""}`}
      >
        <span className="flex items-center text-sm text-gray-900">
          <img src={icon} alt="" />
          <span className="ml-4">{title}</span>
        </span>
        <img
          src={chevronDown}
          className={`ml-auto transition ${open ? "rotate-180" : ""} `}
        />
      </button>
      {open && (
        <div
          className={`overflow-y-scroll w-full transition-all ${open ? "max-h-52" : "max-h-0"}`}
        >
          {isLoading && <p>Loading ...</p>}
          {!isLoading && error && <p className="text-danger-active">{error}</p>}
          {options &&
            options.map((option) => (
              <label
                key={option.label}
                className="flex cursor-pointer items-center w-full px-4 py-2 text-sm text-gray-900"
              >
                <input
                  type="checkbox"
                  checked={selected.includes(option.value)}
                  onChange={() => toggleValue(option.value)}
                  className="checkbox mr-4 w-4 h-4 shrink-0"
                />
                {option.label}
              </label>
            ))}
        </div>
      )}
    </div>
  );
};
