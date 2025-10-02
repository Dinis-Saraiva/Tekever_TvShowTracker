# TV Show Tracker

A full-stack application for browsing, managing, and getting recommendations for TV shows.  

---

## Tech Stack

**Backend**  
- ASP.NET Core  
- Entity Framework Core  
- SQLite  

**Authentication**  
- ASP.NET Core Identity  
- Cookie-based authentication  

**API Layer**  
- REST (for authentication)  
- GraphQL (HotChocolate)  

**Frontend**  
- React  
- React Bootstrap  

**Recommendation Engine**  
- TF-IDF + Cosine Similarity  
- SendGrid for emails  

**Testing**  
- Windows batch scripts with `curl`  

---

## Installation:

1. Clone the repo
  ```sh
  git clone https://github.com/Dinis-Saraiva/Tekever_TvShowTracker.git
  ```
2. Install Packages      
  ```sh
  setup.bat
  ```
3. Swap the .envExample for the .env sent in the email
3. Run Frontend at http://localhost:3000/
  ```sh
  cd tv-show-tracker
  npm start
  ```
4. Run Backend at https://localhost:7211
  ```sh
  cd backend/TvShowTracker.Api
  dotnet run
  ```
5. Run tests
  ```sh
  cd tests
  test_auth_api.bat
  test_favorites_api.bat
  test_tvshow_api.bat
  ```

## My Aproach to the solution:
**1 - User Login/Register**

-I defined the database through C# models ,and made the relationships between them explicit, including the many-to-many links between TV shows, people, episodes, and genres. Then Entity Framework Core transformed automatically these models into a SQLite database with the proper tables and relationships.

**2 - Imported Data**

-I imported the dataset from Kaggle which provided information such as titles, descriptions, genres, durations, and actors/directors. Since the dataset didn’t include episode-level details, I generated these randomly. Likewise, the details for people (actors/directors) were also seeded randomly, with the exception of their connections to shows, which were kept consistent with the original data.

**3 - Identity/Authentication**

-I use ASP.NET Core Identity to handle user registration, login, and roles. Authentication is managed with cookies, which keep users logged in for 60 minutes (TTL). All user data is stored in the database through Entity Framework Core, and authentication requests are handled by the AuthController through REST endpoints. To follow RGPD compliance users can delete their profile and with that their favourite show from the db.

**4 – GraphQL**

-I used GraphQL with HotChocolate for more flexibility when querying data. Implementing paging, filtering, and sorting was straightforward, and I also added caching for the two most requested queries: GetPeople and GetTvShows. Since HotChocolate applies paging and filtering after fetching from the database, caching avoids repeated SQL queries when users scroll through results (e.g., loading more TV shows). For GetTvShows, I flattened the relationship between TV shows and genres using a type descriptor and implemented a DataLoader to further reduce database queries.

**5 - For the frontend** 

-I used React with React Bootstrap to build a clean and responsive interface, and React Context to manage global state like authentication and user info. I also used React Router for navigation between pages such as shows, people and favorites. Users can browse shows and people, navigate between them, and mark or unmark favorites. From the show details page, they can export a PDF with information about that show, and from the favorites page they can trigger the recommendations system to receive personalized emails. The UI was built with reusable components like cards and search bars, and thanks to GraphQL queries, the frontend supports searching, filtering, and paging through TV shows in a flexible way.

**6 – Recommendation System**

-I opted for a TF-IDF vectorization and cosine similarity for content-based recommendations, addressing the cold start problem.I combined Relevant information from each TV show (description, actors, genre, etc.) into a single string, vectorized it, and stored it as sparse JSON vectors (indices[] and values[]) in the database via "TvShowVectorCalculator.cs". When a user requests recommendations, five of their favourite shows are randomly selected, their vectors averaged, then the most similar shows are identified by using cosine similarity and sent to the users Email through sengrid. This can be activated by a button in the favourite tab or background worker every 30 hours.

**7 - For testing**

-I created Windows batch scripts with curl commands to hit the API endpoints and simulate real user flows. With these scripts I tested user registration, login, logout, managing favorites and exporting TV show PDFs.
