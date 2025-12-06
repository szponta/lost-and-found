export interface FormData {
  name: string;
  country: string;
  location: string;
  fromDate: Date;
  toDate: Date;
}

export interface SearchResults {
  items: SearchResultItem[];
}

export interface SearchResultItem {
  id: number;
  name: string;
  description: string;
  status: string;
  eventLocation: string;
  storageLocation: string;
  city: string;
  lostDateFrom?: string;
  lostDateTo?: string;
}
