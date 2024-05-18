# NFL Team Depth Chart Management

This project manages the depth charts for NFL teams. It allows adding and removing players from positions, as well as retrieving backups and the full depth chart.

## Prerequisites

- .NET 8 SDK

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/Lasanga/DepthChartDemo.git

2. Choose main branch
3. Open the sln using visual studio
4. Set up the startup project as FanDuel.DepthChart.ConsoleApp
5. Restore the nuget packages
6. Run the startup project

## Assumptions and notes
1. To make it scalable for new sports, I have introduced a new class called sport. Thereby, sports will have teams, teams will have depth charts and players.
2. Since there is no hints for the data source, I have created 2 data sources, in-memoery and ef-core in-memory. You can switch between them through the keyed service. (Ef for ef core and Local for local in-memory).
3. New sports may have rules different to each other. Therefore, a factory pattern is used to differentiate the logic among them. Used an abstract class so it can have the common logic and can be overriden if needed.
4. Created sports entry in dbContext constructor itself. This should be done through a migration ideally since, adding a sport is not a feature for the users. 
5. Nothing much about the executable. Therefore, used a console application approach than a web api.
6. There are 3 branches, main, v1 and v2. main and v2 is upto date. V1 got the initial commits without ef core
