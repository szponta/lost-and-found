import type { FormData, SearchResults, SearchResultItem } from "../types/search.types";
import modifyDate from "../utils/modifyDate";

const FILE_NAME: string = "search.services.ts";

export const submitSearchRequest = async (
  searchFormData: FormData,
  take: number,
  skip: number
): Promise<SearchResultItem[]> => {
  const params = new URLSearchParams({
    take: take.toString(),
    skip: (skip - 1).toString(),
    search: searchFormData.name,
    foundDateFrom: modifyDate(searchFormData.fromDate),
    foundDateTo: modifyDate(searchFormData.toDate),
    country: searchFormData.country,
    city: searchFormData.city,
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
