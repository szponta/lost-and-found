import { useRef } from "react";
import type { ItemStatusType } from "../types/search.types";
import type { AddFormData } from "../types/add.types.ts";
import FormGroupInput from "./FormGroupInput";
import { submitAddRequest } from "../services/add.services.ts";
import modifyDate from "../utils/modifyDate.ts";
import logObject from "../utils/logObject.ts";

const AddForm = () => {
  const nameRef = useRef<HTMLInputElement | null>(null);
  const statusRef = useRef<HTMLSelectElement | null>(null);
  const eventLocationRef = useRef<HTMLInputElement | null>(null);
  const storageLocationRef = useRef<HTMLInputElement | null>(null);
  const cityRef = useRef<HTMLInputElement | null>(null);
  const countryRef = useRef<HTMLInputElement | null>(null);
  const fromDateRef = useRef<HTMLInputElement | null>(null);
  const toDateRef = useRef<HTMLInputElement | null>(null);
  const formInputsToValidate = [nameRef, statusRef, eventLocationRef, storageLocationRef, cityRef, countryRef];

  const validateFormData = (): boolean => {
    let isValid = true;
    formInputsToValidate.forEach((ref) => {
      if (!ref.current) {
        isValid = false;
        return;
      }

      const current = ref.current;
      if (!current.textContent && !current.value) {
        isValid = false;
        return;
      }
    });

    return isValid;
  };

  const getFormData = (): AddFormData | null => {
    const data: AddFormData = {
      name: nameRef.current?.value || "",
      status: (statusRef.current?.value as ItemStatusType) || "lost",
      eventLocation: eventLocationRef.current?.value || "",
      storageLocation: storageLocationRef.current?.value || "",
      city: cityRef.current?.value || "",
      country: countryRef.current?.value || "",
      lostDateFrom: fromDateRef.current?.value
        ? modifyDate(new Date(fromDateRef.current.value))
        : modifyDate(new Date()),
      lostDateTo: toDateRef.current?.value ? modifyDate(new Date(toDateRef.current.value)) : modifyDate(new Date()),
    };
    return data;
  };
  const handleOnSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    if (!validateFormData()) return;

    const data: AddFormData | null = getFormData();
    if (!data) return;

    logObject(data);

    const result = await submitAddRequest(data);
    console.log("Add Form submission result:", result);
  };

  return (
    <form onSubmit={handleOnSubmit}>
      <FormGroupInput
        id="name"
        ref={nameRef}
        labelText="Nazwa zgubionego przedmiotu"
        placeholder="np. Telefon komórkowy"
        defaultValue="Telefon"
        required
      />

      <select ref={statusRef} id="status" required>
        <option value="lost">Zaginione</option>
        <option value="found">Znalezione</option>
      </select>

      <FormGroupInput
        id="eventLocation"
        ref={eventLocationRef}
        labelText="Miejsce zdarzenia"
        placeholder="np. Park Centralny"
        defaultValue="Park Centralny"
        required
      />

      <FormGroupInput
        id="storageLocation"
        ref={storageLocationRef}
        labelText="Miejsce przechowania"
        placeholder="np. Posterunek policji"
        defaultValue="Posterunek policji"
        required
      />

      <FormGroupInput
        id="city"
        ref={cityRef}
        labelText="Miasto / Miejscowość"
        placeholder="np. Warszawa"
        defaultValue="Warszawa"
        required
      />

      <FormGroupInput
        id="country"
        ref={countryRef}
        labelText="Kraj"
        placeholder="np. Polska"
        defaultValue="Polska"
        required
      />

      <FormGroupInput
        id="fromDate"
        ref={fromDateRef}
        labelText="Data od"
        type="date"
        defaultValue={new Date("2023-12-06").toISOString().split("T")[0]}
        required
      />

      <FormGroupInput
        id="toDate"
        ref={toDateRef}
        labelText="Data do"
        type="date"
        defaultValue={new Date().toISOString().split("T")[0]}
        required
      />

      <button type="submit">Wyślij</button>
    </form>
  );
};

export default AddForm;
