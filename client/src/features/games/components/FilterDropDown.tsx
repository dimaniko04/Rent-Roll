import React, { useState } from "react";

import chevronUp from "@assets/icons/chevronUp.svg";
import chevronDown from "@assets/icons/chevronDown.svg";

interface Props<T extends object> {
  title: string;
  icon: React.ReactNode;
  options: {
    value: T;
    label: string;
  }[];
  selected: T[];
  onSelect: (value: T) => void;
}

export const FilterDropDown = <T extends object>(props: Props<T>) => {
  const [open, setOpen] = useState(false);

  const toggleValue = (value: T) => {
    const newSelected = [...props.selected];
    const index = newSelected.findIndex((v) => v === value);

    if (index !== -1) newSelected.splice(index, 1);
    else newSelected.push(value);

    props.onSelect(value);
  };

  return (
    <div className="w-64 rounded-lg border bg-muted shadow-sm">
      <button
        onClick={() => setOpen(!open)}
        className="flex w-full items-center justify-between gap-2 px-4 py-2"
      >
        <span className="flex items-center gap-2 text-sm font-medium text-gray-800">
          {props.icon}
          <span>{props.title}</span>
        </span>
        {open ? (
          <img src={chevronUp} className="w-4 h-4" />
        ) : (
          <img src={chevronDown} className="w-4 h-4" />
        )}
      </button>
      {open && (
        <div className="flex flex-col gap-2 p-2">
          {props.options.map((option) => (
            <label
              key={option.label}
              className="flex items-center gap-2 px-2 text-sm text-gray-900"
            >
              <input
                type="checkbox"
                checked={props.selected.includes(option.value)}
                onChange={() => toggleValue(option.value)}
                className="form-checkbox h-4 w-4 accent-indigo-600"
              />
              {option.label}
            </label>
          ))}
        </div>
      )}
    </div>
  );
};
