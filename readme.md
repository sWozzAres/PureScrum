# A minimal viable product for Scrum.

## This project is a prototype constructed to explore a potential Scrum system.

The backend is written with a mind to eventually going into production.

The frontend is a throw away implemention. It's
primary purpose is to discover usability features
and explore the structure of the system. 

## Features

1. Timezone aware
2. Uses temporal tables
3. Caching
4. Grpc

Due to it's prototype status, the following important
features are missing...

1. No users
2. Minimal Bootstrap UI 

Other features...

1. CLEAN architecture
2. CQRS
3. Data annotations for validation, allowing both front 
and back end to use the same validation system.
4. Markdown for large text data entry fields
5. Temporal tables to produce Burndown Chart
6. Templated forms to minimize code definition (but Bootstrap reverses
this to some extent)
7. No Mediatr
8. Multi-timezone compatible

Notable features...

1. Toplogical sorting of PBIs with circular reference detection
1. Recursive loading of PBIs to include dependent on PBIs that were not included in the original query

# Files

## Main system

* Scrum.Api - The api with controllers, application and domain
* Scrum.Shared - Http data transfer objects
* Scrum.Web.Api - the api host, also has experimental GRPC services
* Scrum.Web.Blazor - the Blazor client host
* ScrumApp - the Blazor client app
