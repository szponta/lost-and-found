import type { AddFormData } from "../types/add.types.ts";

export const submitAddRequest = async (data: AddFormData) => {
  try {
    const response = await fetch("/api/v1/items", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    const result = await response.json();
    return result;
  } catch (error) {
    console.error("Add request failed:", error);
    return null;
  }
};
