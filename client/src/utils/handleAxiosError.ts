import type { AxiosError } from "axios";

export const handleAxiosError = (
  error: AxiosError
): string | Record<string, string[]> => {
  if (error.response) {
    console.log(error.response);
    console.error("Error response:", error.response.data);

    const data = error.response.data as Record<string, unknown>;

    if ("errors" in data) {
      const errors = data.errors as Record<string, string[]>;

      return errors;
    } else {
      return "title" in data ? (data.title as string) : "An error occurred.";
    }
  } else if (error.request) {
    console.error("Error request:", error.request);
    return "No response from server. Please try again later.";
  } else {
    console.error("Error message:", error.message);
    return "An unexpected error occurred.";
  }
};
