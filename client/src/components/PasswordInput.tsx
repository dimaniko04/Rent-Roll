import {
  useController,
  type FieldValues,
  type UseControllerProps,
} from "react-hook-form";

import view from "@assets/icons/view.png";
import hide from "@assets/icons/hide.png";

interface Props<T extends FieldValues> extends UseControllerProps<T> {
  label: string;
  isVisible: boolean;
  toggleVisibility: () => void;
}

export const PasswordInput = <T extends FieldValues>({
  label,
  isVisible,
  toggleVisibility,
  ...props
}: Props<T>) => {
  const { field, fieldState } = useController(props);

  return (
    <div>
      <div>
        <label htmlFor={props.name} className="text-gray-600 text-sm leading-5">
          {label}
        </label>

        <span className="text-danger float-end">
          {fieldState.error?.message ?? ""}
        </span>
      </div>

      <div className="relative">
        <input
          {...field}
          className="border border-gray-400 py-2 px-2.5 rounded-sm text-sm"
          type={isVisible ? "text" : "password"}
        />
        <img
          src={isVisible ? hide : view}
          alt={isVisible ? "Hide" : "Show"}
          onMouseDown={(e) => e.preventDefault()}
          onClick={toggleVisibility}
          className={`absolute right-0 top-1/2 -translate-y-1/2  p-4 cursor-pointer ${!field.value ? "hidden" : ""}`}
        />
      </div>
    </div>
  );
};
