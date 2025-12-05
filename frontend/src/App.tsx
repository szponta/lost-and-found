import { useEffect } from "react";
import "src/styles/App.css";

const App = () => {
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch("/api/v1/test");
        const data = await response.json();
        console.log(data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);
  return (
    <>
      <h1>Hello World</h1>
    </>
  );
};

export default App;
