const HomePage = () => (
  <div style={{ padding: "2rem", fontFamily: "Arial, sans-serif" }}>
    <h1>Welcome to TV SHOW TRACKER API</h1>
    <p>
      This page was developed from 26/09/2025 to 02/10/2025 as part of the TEKEVER Code Challenge.
    </p>
    <p>
      The purpose of this project was to create a small, RESTful API in .NET for tracking TV shows and actors. 
      The API allows user registration, authentication, and provides features such as listing TV shows and actors, 
      viewing details, filtering by genre or type, managing favorites, and receiving recommendations based on user preferences.
    </p>
    <p>
      The project also includes a React web application demonstrating the API capabilities, with responsive design, pagination, 
      and navigation between shows and actors. Additional features include caching, background workers for recommendations and
      exports to CSV/PDF.
    </p>
    <p>
      Technologies and practices used include SOLID principles, Entity Framework, functional programming, 
      dependency injection, unit & integration tests and GraphQL.
    </p>
    <p>
      This project was done by <a href="https://github.com/Dinis-Saraiva" target="_blank" rel="noopener noreferrer">Dinis Saraiva</a>.
    </p>
  </div>
);

export default HomePage;
