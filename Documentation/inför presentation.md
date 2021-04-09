1. Hur vi tänkt med dokumentation och arbetssätt (dailylog, diagram...) - Daniel
   * Jobbat utifrån user stories, steg för steg
2. Vilka verktyg/hjälpmedel vi använt, hur vi kommunicerat, saker vi kunnat göra annorlunda... - Cecilia
   * Verktyg & Hjälpmedel:
     * Visual Studio
     * Github Desktop
     * Entity Framework
     * Moq.EntityFrameworkCore
     * Moq
     * Discord
     * Slack
     * sv.wikipedia.org/wiki/Fia_(brädspel)
     * Tim Corey på Youtube
     * learnentityframeworkcore.com
     * docs.microsoft.com
     * Draw.io
     * Docker (SQL server container)
     * Azure Data Studio
   * Hade kunnat lagt ännu mer tid på planering
3. Visa koden (delar vi är "stolta" över etc) - Anton
   * Vi har gjort spelmotorn oberoende av gränssnittet, genom att skriva ut till konsolen enbart i Console-projektet. Spelmotorn går då att använda även med ett GUI.
   * Vi använder reflection för att skapa instanser av rätt pjästyp till en spelare.
   * Vi skapade en typ (klass) för varje pjäs (färg), som håller reda på var just den pjästypen (färgen) börjar på spelplanen och var den är i mål.
   * Vi har försökt dela upp koden i mindre metoder för att undvika långa metoder som blir svåra att läsa.
4. Demo (bilder) - Andreas