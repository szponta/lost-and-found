import { useEffect } from "react";
import Navbar from "../components/Navbar";
import Header from "../components/Header";
import "src/styles/Header.css";
import "src/styles/Navbar.css";

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
  return (
    <>
      <Header />
      <Navbar />
      <h1>h</h1>
    </>
  );
};

export default Home;
