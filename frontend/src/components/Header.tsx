const Header = () => {
  return (
    <header>
      <a href="https://www.gov.pl/" id="logo">
        <img src="https://www.gov.pl/img/Herb_Polski.svg" alt="" className="polski_herb" />
        <h3>gov.pl</h3>
      </a>
      <div className="header-center">
        <div id="header-span">
          <span>Serwis Rzeczpospolity Polskiej</span>
        </div>
        <div id="my-gov">
          <a href="https://www.gov.pl/web/gov/klauzula-przetwarzania-danych-osobowych-udostepnionych-droga-elektroniczna">
            <svg role="img" height={"30px"} width={"30px"} fill="white">
              <use href="public/svg/sprite.svg#icon-gov-user"></use>
            </svg>
            <span>Mój gov</span>
          </a>
        </div>
      </div>
      <div id="ue">
        <img src="https://dane.gov.pl/assets/eu-logo/eu-center-pl.svg" alt="Logo uni europejskiej" />
      </div>
    </header>
  );
};

export default Header;
window.location.pathname;
