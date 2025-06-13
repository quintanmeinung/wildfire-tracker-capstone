# Wildfire Tracking Web Application
Capstone Project: Wildfire visualization and evacuation planning tool using NASA FIRMS, Leaflet.js, and Azure SQL.

## URL
https://wildfire-website-app-fnc8bxajf0b0hzf2.canadacentral-01.azurewebsites.net/

## Overview
An interactive wildfire tracking system designed to visualize live fire data, simulate admin-created fires, and assist with evacuation planning. Built with real-time NASA FIRMS and FEMA GIS data.

## Key Features
- Real-time wildfire visualization with radiative power markers
- Toggleable layers: Emergency shelters, AQI, wildfire risk
- Admin-only fire simulation (create/delete custom fire markers)
- User accessibility settings (text size, contrast, text-to-speech)
- Mobile-friendly Leaflet map UI

## Tech Stack
- ASP.NET Core MVC (C#)
- JavaScript + Leaflet.js
- Azure SQL Database
- NASA FIRMS + FEMA ArcGIS APIs
- Reqnroll (Selenium + Gherkin for E2E testing)

## Tech/Presentation Video Link
- https://drive.google.com/file/d/1xSbNU-2iMrbeZaZ9qUAKbv3dKAbC39IF/view?usp=drive_link
- https://drive.google.com/file/d/1KgUCO1YqQcD5l66AkDT4UgNJPSLMpZ3v/view?usp=drive_link
  
## How to Run Locally
1. Clone the repository
2. Configure `appsettings.json` with your local SQL connection or use SQLite
3. Run the project via Visual Studio / `dotnet run`
4. Navigate to `localhost:5000`

## Demo Video
[Link will go here after Friday’s recording]

## Authors
- Quintan Meinung + The Architechs Team @ Western Oregon University
