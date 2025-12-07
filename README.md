# Biuro rzeczy znalezionych - System zarządzania rzeczami zgubionymi i znalezionymi

## Opis projektu

System do zarządzania rzeczami zgubionymi i znalezionymi. Aplikacja integruje dane z Płocka i Olsztyna, umożliwiając użytkownikowi przeszukiwanie dostępnej bazy. Dodatkowo użytkownik może rejestrować nowe zgubione lub znalezione przedmioty poprzez prosty interfejs.

## Rozruch

Z katalogu głównego projektu uruchom:

```bash
docker-compose up --build
```

Aplikacja frontend będzie dostępna pod adresem: `http://localhost:5173`. Backend będzie dostępny pod adresem: `http://localhost:8080/api`. Dodatkowo, swagger UI dla backendu będzie dostępny pod adresem: `http://localhost:8080/api/swagger`.
