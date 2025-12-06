import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

const Navbar = () => {
  const [pathname, setPathname] = useState<string>("");

  useEffect(() => {
    setPathname(getPathName());
  }, [window.location.pathname]);

  const getPathName = () => {
    return window.location.pathname;
  };

  return (
    <nav>
      <ul>
        <li className={pathname === "/" ? "active" : ""}>
          <Link to="/">Home</Link>
        </li>
        <li className={pathname === "/add-lost" ? "active" : ""}>
          <Link to="/add-lost">Dodaj zgubiony</Link>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
