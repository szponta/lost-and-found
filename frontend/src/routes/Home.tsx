import { useState } from "react";
import Navbar from "../components/Navbar";

import SearchForm from "../components/SearchForm.tsx";
import type { SearchResultItem } from "../types/form.types.ts";
import SearchResult from "../components/SearchResult.tsx";

const Home = () => {
  const [searchResult, setSearchResult] = useState<SearchResultItem[]>([]);
  const [itemsPerPage, setItemsPerPage] = useState<number>(10);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [isNextPageAvailable, setIsNextPageAvailable] = useState<boolean>(true);

  return (
    <>
      <Navbar />
      <SearchForm
        setSearchResult={setSearchResult}
        itemsPerPage={itemsPerPage}
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
        setIsNextPageAvailable={setIsNextPageAvailable}
      />
      {searchResult.length > 0 && (
        <SearchResult
          items={searchResult}
          setItemsPerPage={setItemsPerPage}
          setCurrentPage={setCurrentPage}
          currentPage={currentPage}
          isNextPageAvailable={isNextPageAvailable}
        />
      )}
    </>
  );
};

export default Home;
