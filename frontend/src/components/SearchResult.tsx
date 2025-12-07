import type { SearchResultItem } from "../types/search.types";

interface Props {
  items: SearchResultItem[];
  setItemsPerPage: React.Dispatch<React.SetStateAction<number>>;
  setCurrentPage: React.Dispatch<React.SetStateAction<number>>;
  currentPage: number;
  isNextPageAvailable: boolean;
}

const SearchResult = ({ items, setItemsPerPage, setCurrentPage, currentPage, isNextPageAvailable }: Props) => {
  return (
    <div>
      <div>
        {items.map((item) => {
          return (
            <div key={item.id} style={{ border: "5px solid #000" }}>
              <a href={`http://localhost:8080/items/${item.id}`}>{item.title}</a>
              <p>{item.status}</p>
              <p>{item.eventLocation}</p>
              <p>{item.storageLocation}</p>
              <p>{item.city}</p>
              <p>{item.lostDateFrom}</p>
              <p>{item.lostDateTo}</p>
            </div>
          );
        })}
      </div>
      <div>
        <button
          onClick={() => {
            setCurrentPage((prev) => Math.max(prev - 1, 1));
          }}
          disabled={currentPage === 1}
        >
          Previous
        </button>
        <span>Page {currentPage}</span>
        <button
          onClick={() => {
            setCurrentPage((prev) => prev + 1);
          }}
          disabled={!isNextPageAvailable}
        >
          Next
        </button>
      </div>
      <div>
        <label htmlFor="itemsPerPage">Items per page:</label>
        <select
          id="itemsPerPage"
          onChange={(e) => {
            setItemsPerPage(parseInt(e.target.value, 10));
            setCurrentPage(1);
          }}
          defaultValue="10"
        >
          <option value="5">5</option>
          <option value="10">10</option>
          <option value="20">20</option>
          <option value="50">50</option>
        </select>
      </div>
    </div>
  );
};

export default SearchResult;
