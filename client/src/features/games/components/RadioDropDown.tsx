import { useState } from "react";

import chevronDown from "@assets/icons/chevronDown.svg";

interface Props<T> {
  title: string;
  icon: string;
  options: {
    value: T;
    label: string;
  }[];
  selected: T;
  onSelect: (value?: T) => void;
}

export const RadioDropDown = <T,>(props: Props<T>) => {
  const [open, setOpen] = useState(false);

  const toggleValue = (value: T) => {
    if (props.selected === value) {
      props.onSelect();
      return;
    }

    props.onSelect(value);
  };

  return (
    <div className="flex flex-col w-full">
      <button
        onClick={() => setOpen(!open)}
        className={`flex cursor-pointer rounded-lg w-full px-4 py-2 ${open ? "bg-gray-300" : ""}`}
      >
        <span className="flex items-center text-sm text-gray-900">
          <img src={props.icon} alt="" />
          <span className="ml-4">{props.title}</span>
        </span>
        <img
          src={chevronDown}
          className={`ml-auto transition ${open ? "rotate-180" : ""} `}
        />
      </button>

      <div
        className={`overflow-y-hidden w-full transition-all ${open ? "max-h-40" : "max-h-0"}`}
      >
        {props.options.map((option) => (
          <label
            key={option.label}
            className="flex cursor-pointer items-center w-full px-4 py-2 text-sm text-gray-900"
          >
            <input
              type="radio"
              checked={props.selected === option.value}
              onClick={() => toggleValue(option.value)}
              className="radio mr-4 w-4 h-4"
            />
            {option.label}
          </label>
        ))}
      </div>
    </div>
  );
};
