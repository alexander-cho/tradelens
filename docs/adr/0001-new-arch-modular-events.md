# Applying a modular and event-driven architecture

One of the biggest motivations behind this undertaking was the need for an auditable and community-driven data vendor 
solution for researching the financial markets—existing solutions in my experience are sorely missing this. This 
includes third party APIs that offer company fundamental data to comprehensive platforms that readily display said 
data in tables, graphs, charts, machine-readable format, etc. While using these solutions, I often times come across 
errors in the data, especially in companies that I am interested or actively investing in. Now this isn't a deal-breaker
in and of itself because it's a well-known fact that financial data is extremely complex and difficult to unify. However, 
the actionable steps are just as important in creating solutions that help the average investor achieve their goals. 
In existing providers, support is not responsive when inquired or sometimes even aggressive, and erroneous data are left
alone indefinitely as if it were a matter of fact now. A better platform includes a community-driven auditing system 
where users can flag objects from individual data points to leaving general comments, with the expectation of a prompt 
action being taken, whether that is a response from an admin or a complete fix.

The current application at this time has a monolithic structure consisting of 3 layers: a Core class library, an 
Infrastructure class library, and a Web API. Dependencies point outward, and each layer has a purpose. The first
contains the entities (anemic domain models), interfaces (IGenericRepository<T> for data access), and domain services
(MaxPain and options cash calculations), etc. The second provisions access to the databases, third party services,
and implements contracts defined in the Core. The Api provides controlled access to the data for those interested.

For an "enterprise-grade" architecture that has been battle-tested and verified, the above is not without its numerous flaws.
With a clear prospect in mind as detailed previously, I thought it would be a good time to really map out and understand
the moving parts of my domain and their interactions, as well as their coupling.

Consider that a user of the application is viewing the chart for Microsoft's quarterly Cloud revenue breakdown and notices
that the FY2026 Q2 Azure revenue shows as \$32.3 billion but the reality is that the company reported \$32.9 billion. 
They should be able to "flag" this fact and notify users with Admin role/permissions.

----image----

For this problem area, the `AppUser` which is an authenticated user in the User context will be a `Flagger` within this 
Company Fundamentals bounded context. 