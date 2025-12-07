export type ItemStatusType = "found" | "lost";

export interface FormData {
  name: string;
  country: string;
  city: string;
  fromDate: Date;
  toDate: Date;
  status: ItemStatusType;
}

export interface SearchResults {
  items: SearchResultItem[];
}

export interface SearchResultItem {
  id: number;
  title: string;
  status: string;
  eventLocation: string;
  storageLocation: string;
  city: string;
  lostDateFrom: string;
  lostDateTo: string;
}
