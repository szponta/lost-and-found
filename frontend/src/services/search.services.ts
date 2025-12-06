import type { FormData, SearchResults, SearchResultItem } from "../types/form.types";

const FILE_NAME: string = "search.services.ts";

export const submitSearchRequest = async (
  searchFormData: FormData,
  take: number,
  skip: number
): Promise<SearchResultItem[]> => {
  // console.group();
  // for (const key in searchFormData) {
  //   console.log(key.toString(), searchFormData[key as keyof FormData]);
  // }
  // console.groupEnd();

  const params = new URLSearchParams({
    search: searchFormData.name,
    // country: searchFormData.country,
    // location: searchFormData.location,
    // foundDateFrom: searchFormData.fromDate.toISOString(),
    // foundDateTo: searchFormData.toDate.toISOString(),
    take: take.toString(),
    skip: ((take / 2) * skip - 1).toString(),
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
