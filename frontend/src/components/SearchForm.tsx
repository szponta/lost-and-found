import React, { useEffect, useRef, useState } from "react";
import type { FormData, ItemStatusType, SearchResultItem } from "../types/search.types.ts";
import { submitSearchRequest } from "../services/search.services.ts";
import FormGroupInput from "../components/FormGroupInput";

interface Props {
  setSearchResult: React.Dispatch<React.SetStateAction<SearchResultItem[]>>;
  itemsPerPage: number;
  setCurrentPage: React.Dispatch<React.SetStateAction<number>>;
  currentPage: number;
  setIsNextPageAvailable: React.Dispatch<React.SetStateAction<boolean>>;
}

const SearchForm = ({ setSearchResult, itemsPerPage, currentPage, setCurrentPage, setIsNextPageAvailable }: Props) => {
  const nameRef = useRef<HTMLInputElement | null>(null);
  const countryRef = useRef<HTMLSelectElement | null>(null);
  const locationRef = useRef<HTMLInputElement | null>(null);
  const fromDateRef = useRef<HTMLInputElement | null>(null);
  const toDateRef = useRef<HTMLInputElement | null>(null);
  const statusRef = useRef<HTMLSelectElement | null>(null);
  const formInputsToValidate = [nameRef, countryRef, statusRef];
  const [lastQuery, setLastQuery] = useState<FormData | null>(null);

  useEffect(() => {
    if (!lastQuery) return;
    const repeatRequest = async () => {
      const items = (await submitSearchRequest(lastQuery, itemsPerPage * 2, currentPage)) as SearchResultItem[];
      setIsNextPageAvailable(items.length - itemsPerPage > 0);
      setSearchResult(items.slice(0, itemsPerPage));
    };
    repeatRequest();
  }, [currentPage]);

  useEffect(() => {
    if (!lastQuery) return;
    const repeatRequest = async () => {
      const items = (await submitSearchRequest(lastQuery, itemsPerPage * 2, currentPage)) as SearchResultItem[];
      setIsNextPageAvailable(items.length - itemsPerPage > 0);
      setSearchResult(items.slice(0, itemsPerPage));
    };
    repeatRequest();
    setCurrentPage(1);
  }, [itemsPerPage]);

  const validateFormData = (): boolean => {
    let isValid = true;
    formInputsToValidate.forEach((ref) => {
      if (!ref.current) {
        isValid = false;
        return;
      }
      const current = ref.current;
      if (!current.textContent && !current.value) {
        console.log(current, "empty value");
        isValid = false;
        return;
      }
    });
    return isValid;
  };

  const getFormData = (): FormData | null => {
    const data: FormData = {
      name: nameRef.current?.value || "",
      country: countryRef.current?.value || "",
      location: locationRef.current?.value || "",
      fromDate: fromDateRef.current?.value ? new Date(fromDateRef.current.value) : new Date(),
      toDate: toDateRef.current?.value ? new Date(toDateRef.current.value) : new Date(),
      status: (statusRef.current?.value as ItemStatusType) || "lost",
    };
    return data;
  };

  const handleOnSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    if (!validateFormData()) return;

    const data: FormData | null = getFormData();
    if (!data) return;

    const items = (await submitSearchRequest(data, itemsPerPage * 2, currentPage)) as SearchResultItem[];
    setIsNextPageAvailable(items.length - itemsPerPage > 0);
    setLastQuery(data);
    setCurrentPage(1);
    setSearchResult(items.slice(0, itemsPerPage));
  };

  return (
    <form onSubmit={handleOnSubmit}>
      <FormGroupInput
        id="name"
        ref={nameRef}
        labelText=""
        placeholder="Nazwa zgubionego przedmiotu"
        defaultValue="Telefon Samsung"
        required
      />

      <select ref={countryRef} id="country" required>
        <option value="polska">Polska</option>
        <option value="niemcy">Niemcy</option>
        <option value="czechy">Czechy</option>
      </select>

      <FormGroupInput
        id="location"
        ref={locationRef}
        labelText="Miejscie gdzie przedmiot został zgubiony"
        placeholder="np. Płock"
        defaultValue="Płock"
      />

      <FormGroupInput
        id="fromDate"
        ref={fromDateRef}
        labelText="Data od"
        type="date"
        defaultValue={new Date("2023-12-06").toISOString().split("T")[0]}
      />
      <FormGroupInput
        id="toDate"
        ref={toDateRef}
        labelText="Data do"
        type="date"
        defaultValue={new Date().toISOString().split("T")[0]}
      />

      <select ref={statusRef} id="status" required>
        <option value="lost">Zaginione</option>
        <option value="found">Znalezione</option>
      </select>

      <button type="submit">Wyślij</button>
    </form>
  );
};

export default SearchForm;
