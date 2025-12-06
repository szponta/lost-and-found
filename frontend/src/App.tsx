import { Route, Routes } from "react-router-dom";
import "src/styles/App.css";
// Routes
import Home from "./routes/Home";
import SearchResultPage from "./routes/SearchResultPage";
import AddLostPage from "./routes/AddLostPage";

const App = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/search-result" element={<SearchResultPage />} />
      <Route path="/add-lost" element={<AddLostPage />} />
      <Route path="*" element={<Home />} />
    </Routes>
  );
};

export default App;
