import type { FormData, ItemsResponse } from "../types/form.types";

const FILE_NAME: string = "search.services.ts";

export const submitSearchRequest = async (
  searchFormData: FormData,
  take: number = 10,
  skip: number = 0
): Promise<ItemsResponse | null> => {
  console.log("submitSearchRequest called");
  console.group();
  for (const key in searchFormData) {
    console.log(key.toString(), searchFormData[key as keyof FormData]);
  }
  console.groupEnd();

  const params = new URLSearchParams({
    name: searchFormData.name,
    country: searchFormData.country,
    location: searchFormData.location,
    fromDate: searchFormData.fromDate.toISOString(),
    toDate: searchFormData.toDate.toISOString(),
    take: take.toString(),
    skip: skip.toString(),
  });

  const URL: string = `/api/v1/items/${params.toString()}`;

  try {
    const response = await fetch(URL, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    const data: ItemsResponse = await response.json();

    console.group();
    for (const key in data) {
      console.log(key.toString(), data[key as keyof ItemsResponse]);
    }
    console.groupEnd();

    return data;
  } catch (error: any) {
    console.error(error.message, FILE_NAME, "submitSearchRequest");
  }

  return null;
};
