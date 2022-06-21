# bd2projectApiTest

Jak uruchomić projekt?

- Otwórz Microsoft SQL Server Management Studio
- Uruchom kod SQL znajdujący się w pliku createDatabase.sql
- Otwórz bd2_projekt-master/utd_point.sln w Visual Studio
- Kliknij start
- W katalogu jest plik udt_point.publish.xml, opublikuj kod napisany w c# za pomocą tego pliku. 
  Upewnij się że nazwa bazy danych jest taka sama jak w skrypcie createDatabase.sql
- Otwórz bd2projectApiTest-master/projectApiTest.sln w Visual Studio
- Kliknij start. Zostaną wykonane unit testy.
- Koniec
