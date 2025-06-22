/* import { useState } from "react";

import chevronUp from "@assets/icons/chevronUp.svg";
import chevronDown from "@assets/icons/chevronDown.svg";

interface Props {
  title: string;
  icon: React.ReactNode;
  selected: string[];
  onSelect: (selected: string[]) => void;
}

export function AsyncFilterDropDown({
  title,
  icon,
  selected,
  onSelect,
}: Props) {
  const [open, setOpen] = useState(false);

  const toggleValue = (value: string) => {
    const newSelected = [...selected];
    const index = newSelected.indexOf(value);

    if (index !== -1) newSelected.splice(index, 1);
    else newSelected.push(value);

    onSelect(newSelected);
  };

  return (
    <div className="w-64 rounded-lg border bg-muted shadow-sm">
      <button
        onClick={() => setOpen(!open)}
        className="flex w-full items-center justify-between gap-2 px-4 py-2"
      >
        <span className="flex items-center gap-2 text-sm font-medium text-gray-800">
          {icon}
          <span>{title}</span>
        </span>
        {open ? (
          <img src={chevronUp} className="w-4 h-4" />
        ) : (
          <img src={chevronDown} className="w-4 h-4" />
        )}
      </button>
      {open && (
        <div className="flex flex-col gap-2 p-2">
          {.map((value) => (
            <label
              key={value}
              className="flex items-center gap-2 px-2 text-sm text-gray-900"
            >
              <input
                type="checkbox"
                checked={selected.includes(value)}
                onChange={() => toggleValue(value)}
                className="form-checkbox h-4 w-4 accent-indigo-600"
              />
              {value}
            </label>
          ))}
        </div>
      )}
    </div>
  );
}
 */
