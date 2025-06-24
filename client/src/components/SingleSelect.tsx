import {
  Controller,
  type Control,
  type FieldValues,
  type Path,
} from "react-hook-form";
import Select from "react-select";

interface Props<T extends FieldValues> {
  label: string;
  name: Path<T>;
  control: Control<T>;
  options: { label: string; value: string }[];
}

export const SingleSelect = <T extends FieldValues>({
  label,
  name,
  control,
  options,
}: Props<T>) => {
  const fieldState = control.getFieldState(name);

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
                  "border border-gray-400 py-2 px-2.5 rounded-sm text-sm w-full": true,
                }),
              }}
              options={
                options?.map((option) => ({
                  label: option.label,
                  value: option.value,
                })) || []
              }
              value={options.find((option) => option.value === field.value)}
              onBlur={field.onBlur}
              onChange={(option) => field.onChange(option?.value)}
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
