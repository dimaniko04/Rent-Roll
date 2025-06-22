import type { HTMLInputTypeAttribute } from "react";
import {
  useController,
  type FieldValues,
  type UseControllerProps,
} from "react-hook-form";

interface Props<T extends FieldValues> extends UseControllerProps<T> {
  label: string;
  type?: HTMLInputTypeAttribute;
}

export const TextInput = <T extends FieldValues>({
  label,
  type = "text",
  ...props
}: Props<T>) => {
  const { field, fieldState } = useController(props);

  return (
    <div>
      <label htmlFor={props.name} className="text-gray-600 text-sm leading-5">
        {label}
      </label>

      <div>
        <div>
          <input
            id={props.name}
            type={type}
            className="border border-gray-400 py-2 px-2.5 rounded-sm text-sm w-full"
            {...field}
          />
        </div>
        <span
          className={`text-danger float-end text-xs ${fieldState.error ? "visible" : "invisible"}`}
        >
          {fieldState.error?.message || ""}
        </span>
      </div>
    </div>
  );
};
