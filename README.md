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

## Architecture Diagram


```mermaid
graph TD;
    A[Browser<br>Client] --> B[Web Server<br>Azure Web App]
    B --> C[Application Server<br>ASP.NET Razor Pages]
    C --> E[Authentication Service<br>Auth]
    E --> F[Microsoft Entra ID<br>Identity]
    C --> G[Azure Functions<br>Serverless]
    C --> H[Cosmos DB<br>Database]
    C --> K[Application Insights<br>Monitoring]
