import type { FormData, SearchResults, SearchResultItem } from "../types/form.types";

const FILE_NAME: string = "search.services.ts";

const modifyDate = (date: Date): string => {
  return date.toISOString().split("T")[0];
};

export const submitSearchRequest = async (
  searchFormData: FormData,
  take: number,
  skip: number
): Promise<SearchResultItem[]> => {
  const params = new URLSearchParams({
    take: take.toString(),
    skip: ((take / 2) * skip - 1).toString(),
    search: searchFormData.name,
    foundDateFrom: modifyDate(searchFormData.fromDate),
    foundDateTo: modifyDate(searchFormData.toDate),
    country: searchFormData.country,
    // location: searchFormData.location, // 500
  });

  const URL: string = `/api/v1/items/?${params.toString()}`;

  try {
    const response = await fetch(URL, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data: SearchResults = await response.json();
    return data.items;
  } catch (error: any) {
    console.error(error.message, FILE_NAME, "submitSearchRequest");
  }
  return [];
};
