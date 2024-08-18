# NestFinder

NestFinder is a web application that allows users to search, list, and manage properties. Built with Angular and ASP.NET Core Web API, it provides a robust platform for real estate management.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- Search properties by location, price, and type.
- Add and manage property listings.
- User authentication and authorization.
- Responsive design.
- Property filtering and sorting.
- Interactive map integration.

## Tech Stack

- **Frontend**: Angular 16, Bootstrap
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server 2022
- **Tools**: Visual Studio, SQL Server Management Studio (SSMS)
- **Other**: ngx-gallery for image galleries, AlertifyJS for notifications

## Getting Started

### Prerequisites

- Node.js and npm installed
- Angular CLI installed
- .NET 6 SDK installed
- SQL Server 2022

### Installation

1. Clone the repository:

    ```bash
   git clone https://github.com/saty051/NestFinder.git


2. Navigate to the project directory:

    ```bash
    cd NestFinder


3. Install frontend dependencies:

    ```bash
    cd FrontEnd
    npm install

4. Build the Angular application:

    ```bash
    ng build

5. Set up the backend:

Navigate to the BackEnd/WebAPI directory.
Open the solution in Visual Studio.
Restore NuGet packages.
Update appsettings.json with your database connection string.
   
6. Run the application:

Start the backend API from Visual Studio.

Serve the frontend with Angular CLI:
    ```bash
    ng serve
    The application should be running on http://localhost:4200/.
    


### Usage
Property Listings
   * Users can view, search, and filter properties.
   *  Authenticated users can add new property listings.

 User Authentication
   * Register and log in to access restricted features.

Admin Panel
    * Admin users can manage all property listings and user accounts.    


### API Documentation
The backend API is built with ASP.NET Core Web API and provides various endpoints for managing properties, users, and other resources.


### Contributing
Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (git checkout -b feature/your-feature-name).
3. Commit your changes (git commit -m 'Add some feature').
4. Push to the branch (git push origin feature/your-feature-name).
5. Open a Pull Request.


### License
This project is licensed under the MIT License - see the LICENSE file for details.
[MIT License](LICENSE)

### Contact
For any inquiries or feedback, feel free to contact:

* Name: Satyam Verma
* GitHub: saty051
* Email: satyamverma0102@gmail.com

