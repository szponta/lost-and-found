import React, { useEffect } from "react";
import Navbar from "../components/Navbar";
import FormGroupInput from "../components/FormGroupInput";

const Home = () => {
  useEffect(() => {
    const fetchData = async () => {
      try {
        // const response = await fetch("/api/v1/test");
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);

  const handleOnSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
  };

  return (
    <>
      <Navbar />
      <form onSubmit={handleOnSubmit}>
        <FormGroupInput
          id="search-form-item-name"
          labelText="Nazwa zgubionego przedmiotu"
          placeholder="np. Telefon komórkowy"
        />
        <FormGroupInput
          id="search-form-item-location"
          labelText="Miejscie gdzie przedmiot został zgubiony"
          placeholder="np. Warszawa"
        />
        <span>Maksymalnie tydzień</span>
        <FormGroupInput id="search-form-date-from" labelText="Data od" type="date" />
        <FormGroupInput id="search-form-date-to" labelText="Data do" type="date" />
        <button type="submit">Wyślij</button>
      </form>
    </>
  );
};

export default Home;
