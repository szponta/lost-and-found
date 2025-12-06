import React, { useRef } from "react";
import Navbar from "../components/Navbar";
import FormGroupInput from "../components/FormGroupInput";
import type { FormData } from "../types/form.types.ts";
import { submitSearchRequest } from "../services/search.services.ts";

const Home = () => {
  const nameRef = useRef<HTMLInputElement | null>(null);
  const countryRef = useRef<HTMLSelectElement | null>(null);
  const locationRef = useRef<HTMLInputElement | null>(null);
  const fromDateRef = useRef<HTMLInputElement | null>(null);
  const toDateRef = useRef<HTMLInputElement | null>(null);
  const formInputsRefs = [nameRef, countryRef, locationRef, fromDateRef, toDateRef];

  const validateFormData = (): boolean => {
    let isValid = true;
    formInputsRefs.forEach((ref) => {
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
      name: "",
      country: "",
      location: "",
      fromDate: new Date(),
      toDate: new Date(),
    };

    const keys: string[] = [];
    formInputsRefs.forEach((ref) => {
      if (!ref.current?.id) return;
      keys.push(ref.current.id);
    });

    keys.forEach((id) => {
      const ref = formInputsRefs.find((r) => r.current?.id === id);
      if (!ref || !ref.current) return;

      const current = ref.current;
      if (id in data) {
        const key = id as keyof FormData;
        if (key === "fromDate" || key === "toDate") {
          data[key] = new Date(current.value);
        } else {
          data[key] = current.value;
        }
      }
    });

    return data;
  };

  const handleOnSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    if (!validateFormData()) return;

    const data: FormData | null = getFormData();
    if (!data) return;

    const result = await submitSearchRequest(data);
  };

  return (
    <>
      <Navbar />
      <form onSubmit={handleOnSubmit}>
        <FormGroupInput
          id="name"
          ref={nameRef}
          labelText="Nazwa zgubionego przedmiotu"
          placeholder="np. Telefon komórkowy"
          defaultValue="Telefon"
        />

        <select ref={countryRef} id="country">
          <option value="polska">Polska</option>
          <option value="niemcy">Niemcy</option>
          <option value="czechy">Czechy</option>
        </select>

        <FormGroupInput
          id="location"
          ref={locationRef}
          labelText="Miejscie gdzie przedmiot został zgubiony"
          placeholder="np. Warszawa"
          defaultValue="Warszawa"
        />
        <span>Maksymalnie tydzień</span>
        <FormGroupInput
          id="fromDate"
          ref={fromDateRef}
          labelText="Data od"
          type="date"
          defaultValue={new Date().toISOString().split("T")[0]}
        />
        <FormGroupInput
          id="toDate"
          ref={toDateRef}
          labelText="Data do"
          type="date"
          defaultValue={new Date().toISOString().split("T")[0]}
        />
        <button type="submit">Wyślij</button>
      </form>
    </>
  );
};

export default Home;
