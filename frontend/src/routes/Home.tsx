import { useEffect } from "react";

const Home = () => {
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
  return <h1>h</h1>;
};

export default Home;
