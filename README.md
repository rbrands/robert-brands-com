![Deploy "master" to slot "dev"](https://github.com/rbrands/robert-brands-com/workflows/Deploy%20%22master%22%20to%20slot%20%22dev%22/badge.svg)

# Personal CMS for robert-brands.com

This site is my personal website and provides a very simple CMS. I use it to share photos, roadbike tracks, travel tips, etc. But first of all, it demonstrates some features around development with:

- ASP.NET Core with Razor Pages
- Authentication with Microsoft Entra ID
- NoSQL database Azure Cosmos DB
- Azure Web-Apps
- Responsive Design with Bootstrap
- GitHub Actions
- Using Azure Functions with fluent API

Feel free to use it as a start for your own development or to understand and see examples of the mentioned technologies.

For further documentation see the Wiki https://github.com/rbrands/robert-brands-com/wiki

## Component Diagram

```mermaid
graph TD;
    A[Browser] --> B[Web Server]
    B --> C[Application Server]
    C --> D[Database]
    C --> E[Authentication Service]
    E --> F[Azure AD]
    C --> G[Azure Functions]
    C --> H[Cosmos DB]

    
