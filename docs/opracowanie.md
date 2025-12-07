
## Treść zadania

> Stwórz mechanizm w portalu dane.gov.pl, który ułatwi samorządom szybkie wgrywanie do portalu dane.gov.pl danych dotyczących rzeczy znalezionych i zgromadzenie ich w jednym miejscu.

Mamy wyszukiwanie po konkretnym rekordzie, natomiast nie mamy listowania wszystkich zestronicowanych pozycji.

### 1. Wprowadzenie - opis organizacji, sytuacji i stanu aktualnego 

> Zdarzyło Wam się zgubić coś w autobusie i zastanawialiście się, gdzie zguby szukać? Samorządy prowadzą rejestry rzeczy znalezionych na ich terenie, ale dane te są rozproszone po wielu stronach – w Biuletynach Informacji Publicznej poszczególnych powiatów. Taki model utrudnia szybkie odnalezienie poszukiwanej rzeczy. Dlatego podczas HackNation chcemy to uprościć poprzez udostępnienie w portalu dane.gov.pl danych o rzeczach znalezionych. Pozwoli to na stworzenie wspólnej bazy rzeczy znalezionych i ułatwi poszukiwania zagubionych przedmiotów. Dane będą 
udostępnione w portalu dane.gov.pl w formie dostępnej publicznie, bezpłatnie, w formacie czytelnym maszynowo oraz umożliwiającym przetwarzanie. To rozwiązanie stworzy nowe możliwości dla innowacji technologicznych. Otworzy przestrzeń do tworzenia aplikacji i narzędzi wspierających szukanie zagubionych rzeczy (np. do natychmiastowego wyszukiwania przedmiotów, powiadomień o znalezieniu rzeczy w okolicy czy integracji z mapami i systemami zgłoszeń).
> 
> To szansa na stworzenie czegoś dobrego. Bądź częścią tego procesu.

Mamy eksport do postaci JSON.

### 2. Wyzwanie  

> Stwórz mechanizm w portalu dane.gov.pl - który ułatwi samorządom szybkie (max. 5 kroków) udostępnianie danych dotyczących rzeczy znalezionych. To na jaki typ rozwiązania postawisz, zależy od Ciebie. Pamiętaj, aby było dostępne, responsywne, proste w użytkowaniu i zgodne ze standardami portalu dane.gov.pl.

Mamy jeden krok dodawania danych przez urzędnika, ale bez możliwości uzupełniania szczegółowych informacji o parametrach znalezionej rzeczy. Jeśli chodzi o responsywność to się nie rozjeżdża na telefonie, ale szału nie ma.

> Dodatkowo, zaprojektuj wzorcowy zakres takich danych do udostępnienia w portalu, aby wszystkie samorządy udostępniały dane w jednolity sposób.

Należy wyeksportować kontrakty.

> Dla kogo to rozwiązanie? Finalnie dla nas wszystkich, którzy czasem coś gubimy. Użytkownikiem samego rozwiązania technicznego jest urzędnik, który nie jest informatykiem/programistą.

Załóżmy że urzędnik jest w stanie obsługiwać tę stronę.

### 3. Oczekiwany rezultat

> Prototyp rozwiązania - „jednego okna” do udostępniania danych dotyczących rzeczy znalezionych przez samorządy w portalu dane.gov.pl. 
 
> Cel: urzędnik jest w stanie w max. 5 krokach udostępnić dane (o jednolitym zakresie) w portalu dane.gov.pl.

Mamy jeden krok formularza, bardzo elementarnej formy.

### 4. Wymagania formalne  

> Projekt przesyłany do oceny powinien zawierać: 
> - prezentację  
>   - min. 5 slajdów z dowodami na realizację wymagań technicznych (zrzuty ekranu);  
>   - opis funkcjonalności, w tym opis kroków użytkownika;  
>   - lista ewentualnych ograniczeń (o ile wystąpiły); 

Czekamy na prezentację.

> - działający prototyp mechanizmu (np. demo, link, wideo 2–3 min);

Mamy demo z docker compose, można nagrać wideo użytkowania.

> - plik z wzorcowym zakresem danych do udostępniania danych w portalu;

Do przygotowania.

> - kod źródłowy.

Mamy.

### 5. Wymagania techniczne  
> - UX - rozwiązanie przyjazne dla urzędnika, max. 5 krokach do udostępnienia danych,

;)

> - Wymóg formatu odczytywalnego maszynowo dla udostępnianych danych w opracowanym rozwiązaniu (niedopuszczalne np. pdf, doc),

Mamy eksport do JSON, ale bez linku na stronie. Należy dodać link na stornie.

> - Zgodność z zasadami działania portalu dane.gov.pl.

Brak.

> Dodatkowo projektując rozwiązanie należy pamiętać o zgodności z dostępnością WCAG 2.1 oraz responsywności, tj. poprawne działanie na komputerach i urządzeniach mobilnych.

Jest responsywne, ale wymaga poprawek UX.

### 6. Sposób testowania i/lub walidacji

Uruchomienie prototypu mechanizmu i weryfikacja wymogów technicznych i funkcjonalnych

### 7. Dostępne zasoby 
- Otwarty kod portalu dane.gov.pl https://dane.gov.pl/source-code/ 
- Baza wiedzy portalu dane.gov.pl https://dane.gov.pl/pl/knowledgebase/useful-materials 
 
### 8. Kryteria oceny  
> - Zgodność z zasadami działania portalu dane.gov.pl - 35% 

Brak.

> - Kreatywność i innowacyjność rozwiązania - 25% 

Prosty CRUD jest raczej mało innowacyjny.

> - UX & UI rozwiązania - 20% 

Zależy od opinii.

> - Wzorcowy zakres danych o rzeczach znalezionych uwzględniających potrzeby użytkowników portalu – 15% 

Kontrakt danych - do przygotowania.

> - Prezentacja wypracowanego i kompletnego rozwiązania - 5%

Prezentacja do przygotowania.

### 9. Dodatkowe uwagi / kontekst wdrożeniowy

> Wybrane rozwiązanie zostanie włączone do planów rozwojowych portalu.

Myślę że to nas nie dotyczy.

### 10. Kontakt

> Bieżące wsparcie zapewnimy zarówno na stoisku danych oraz wsparcie mentorskie w wiosce mentorskiej. 
 
Następnym razem warto będzie z tego skorzystać.
 