# Lost and Found

System do zarządzania rzeczami zgubionymi i znalezionymi. Mała idea, milion realizacji.

## Propozycje

1. Pomińmy w MVP proces rejestracji i logowania, wiemy że to zabiera dużo czasu, a generalnie jest strasznie upierdliwe.
2. Zamiast rzeźbienia całego CRUDa zróbmy tylko Create + Read, ew. aktualizację statusu jak nasza rzecz się znajdzie.
3. Generalnie, zależnie od rodzaju rzeczy - np. laptop, telefon, portfel, kurna nawet zwierz - mamy kompletnie różne obszary i kategorie. Ciężko jest tu się połapać.
4. Dobrą propozycją jest integracja z Google Mapi API, to nam od razu podnosi coolness projektu.
5. Stworzyć tagi/kategorie, wraz z podkategoriami etc, etc, które można uzupełniać i w ten sposób kategoryzować zgubione/znalezione rzeczy.
6. Możliwość dodawania zdjęć do zgłoszeń.
7. Zestaw pytań kontrolnych typu "jakiego koloru była rzecz?", "jaki model telefonu", "czy miała jakieś charakterystyczne cechy?" itp.
8. Powiadomienia email/SMS dla osób, które zgłosiły zgubę. To można załatwić przez jakąś prostą integrację z zewnętrznym serwisem.

## Przemyślenia

Generalnie, rozwiązanie ma być sensowne. Najprostsza funkcjonalność biura rzeczy znalezionych - wywieszona karteczka "znaleziono X/zgubiono Y, proszę o kontakt Z".

Jeśli byśmy robili to w większej skali, to byłby wtedy arkusz excelowy z listą rzeczy znalezionych vs arkusz z listą rzeczy zgubionych. To też miałoby sens, pod warunkiem że ktoś siedziałby ciągle na telefonie i przeglądał ten arkusz.

Czyli strona miałaby rozszerzać arkusz kalkulacyjny o możliwość szybkiego sprawdzenia "czy ktoś znalazł telefon w czasie xyz"? Albo "czy ktoś zgubił portfel w okolicy abc"?

## Słownik domenowy

- Rzecz zgubiona (Lost Item) - przedmiot, który został zgubiony przez właściciela.
- Rzecz znaleziona (Found Item) - przedmiot, który został znaleziony przez osobę trzecią.
- Zgłoszenie (Report) - proces rejestracji rzeczy zgubionej lub znalezionej w systemie.
- Kategoria (Category) - klasyfikacja rzeczy zgubionych i znalezionych, np. elektronika, odzież, dokumenty itp.
- Lokalizacja (Location) - miejsce, gdzie rzecz została zgubiona lub znaleziona.
- Status (Status) - aktualny stan rzeczy, np. "zgubiona", "znaleziona", "odebrana".
- Użytkownik (User) - osoba korzystająca z systemu do zgłaszania rzeczy zgubionych lub znalezionych.
- Powiadomienie (Notification) - informacja wysyłana do użytkownika o zmianie statusu jego zgłoszenia lub innych istotnych wydareniach.
- Zdjęcie (Photo) - wizualne przedstawienie rzeczy zgubionej lub znalezionej, dołączone do zgłoszenia.
- Tagi (Tags) - słowa kluczowe przypisane do zgłoszeń w celu ułatwienia wyszukiwania i kategoryzacji.
- Data zgubienia/znalezienia (Date Lost/Found) - konkretna data, kiedy rzecz została zgubiona lub znaleziona.
- Kontakt (Contact) - informacje umożliwiające skontaktowanie się z osobą zgłaszającą rzecz zgubioną lub znalezioną.
- Opis (Description) - szczegółowe informacje dotyczące rzeczy zgubionej lub znalezionej, pomagające w jej identyfikacji.

- Lost Item/Found Item - Item Record - ogólna abstrakcja reprezentująca zarówno zgubione, jak i znalezione przedmioty.
- Item Record Event - zdarzenie związane z przedmiotem, takie jak jego zgubienie, znalezienie, aktualizacja statusu itp.

Jakie są zdarzenia w systemie?

- Item Reported Lost - zdarzenie rejestracji zgubionego przedmiotu.
- Item Reported Found - zdarzenie rejestracji znalezionego przedmiotu
- Item Reported Recovered - zdarzenie aktualizacji statusu przedmiotu na "odebrany" lub "odzyskany".

- Owner Notified About Found Item - zdarzenie powiadomienia właściciela o znalezieniu jego zgubionego przedmiotu.
- Finder Notified About Lost Item - zdarzenie powiadomienia znalazcy o zgubieniu przedmiotu przez właściciela.

- Owner Contacted Finder - zdarzenie, gdy właściciel skontaktuje się ze znalazcą w sprawie odzyskania przedmiotu.
- Finder Contacted Owner - zdarzenie, gdy znalazca skontaktuje się z właścicielem w sprawie zwrotu przedmiotu.

- Item Recovered - zdarzenie oznaczające, że przedmiot został pomyślnie odzyskany przez właściciela.
- Item Not Recovered - zdarzenie oznaczające, że przedmiot nie został odzyskany.

Jakie są interakcje użytkownika z systemem?

- Report Lost Item - użytkownik zgłasza zgubiony przedmiot.
- Report Found Item - użytkownik zgłasza znaleziony przedmiot.
- Contact Finder/Owner - użytkownik kontaktuje się ze znalezionym/właścicielem przedmiotu.
- Update Item Status - użytkownik aktualizuje status przedmiotu (np. na "odebrany").
- Search for Lost/Found Items - użytkownik przeszukuje bazę zgubionych/znalezionych przedmiotów.
- View Item Details - użytkownik przegląda szczegóły zgubionego/znalezionego przedmiotu.

Jakie są domenowe reguły biznesowe?

- Lost items must be reported within a specified time frame to be eligible for recovery.
- Found items must be reported with accurate location and time details.
- Users must provide valid contact information when reporting lost or found items.
- Items reported as lost can only be marked as recovered by the original owner.
- Notifications must be sent to users when the status of their reported items changes.
- Items must be categorized appropriately to facilitate search and retrieval.
- Photos attached to item reports must meet specified quality standards.
- Users can only report items that they have personally lost or found.
- The system must maintain a log of all interactions and status changes for each item.
