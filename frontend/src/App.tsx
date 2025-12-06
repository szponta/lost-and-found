import { Route, Routes } from "react-router-dom";
import "src/styles/App.css";
import Home from "./routes/Home";

const App = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
    </Routes>
  );
};

export default App;
