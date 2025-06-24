import { useForm, useWatch } from "react-hook-form";
import type { GamePrice } from "../../types/GameDetails";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { TextInput } from "@/components/TextInput";
import { AxiosError } from "axios";
import { toast } from "react-toastify";
import { SingleSelect } from "@/components/SingleSelect";
import { useMutation } from "@tanstack/react-query";
import { RentalService } from "@/features/rentals/services/RentalService";

interface Props {
  id: string;
  type: string;
  prices: GamePrice[];
  onClose: () => void;
}

interface RentalFormData {
  term: string;
  quantity: number;
}

const RentalFormSchema = z.object({
  term: z.string().nonempty("Required"),
  quantity: z.number().min(1, "Must be at least 1"),
});

export const RentalForm = ({ prices, id, type, onClose }: Props) => {
  const { control, handleSubmit, reset } = useForm<RentalFormData>({
    defaultValues: {
      term: prices[0].timeUnit,
      quantity: 1,
    },
    mode: "onBlur",
    resolver: zodResolver(RentalFormSchema),
  });

  const loginMutation = useMutation({
    mutationFn: async (values: RentalFormData) => {
      return await RentalService.create({
        id,
        type,
        unit: values.term as "hour" | "day" | "week" | "month",
        term: values.quantity,
      });
    },
    onError: (error: Error) => {
      if (error instanceof AxiosError) {
        const axiosError = error as AxiosError;

        if ("response" in axiosError) {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          const response = axiosError.response as any;
          if (response?.status === 404) {
            toast.error("Game not found");
            onClose();
            return;
          } else {
            toast.error(
              `Error loading game details: ${response?.status} ${response.data.message}`
            );
            onClose();
            return;
          }
        }
      }

      toast.error(`Error loading game details: ${error.message}`);
      onClose();
    },
    onSuccess: () => {
      toast.success("Rental created successfully");
      reset();
      onClose();
    },
  });

  const [term, quantity] = useWatch({
    control,
    name: ["term", "quantity"],
  });

  const onSubmit = (data: RentalFormData) => {
    loginMutation.mutate(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
      <SingleSelect
        label="Term"
        name="term"
        control={control}
        options={prices.map((price) => ({
          label: `By ${price.timeUnit}`,
          value: price.timeUnit,
        }))}
      />

      <TextInput
        label="Quantity"
        name="quantity"
        control={control}
        type="number"
      />

      <button
        type="submit"
        className="p-3 cursor-pointer w-full text-white text-lg bg-primary rounded-md"
      >
        Rent $
        {(quantity * (prices.find((p) => p.timeUnit === term)?.price || 0)) /
          100}
      </button>
    </form>
  );
};
