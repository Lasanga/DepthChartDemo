# NFL Team Depth Chart Management

This project manages the depth charts for NFL teams. It allows adding and removing players from positions, as well as retrieving backups and the full depth chart.

## Prerequisites

- .NET 8 SDK

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/Lasanga/DepthChartDemo.git

2. Open the sln using visual studio
3. Set up the startup project as FanDuel.DepthChart.ConsoleApp
4. Restore the nuget packages
5. Run the startup project

## Assumptions and notes
1. Its mentioned that there will be 32 depthcharts per season. But since its not based as a requirement, only the first week is returned by default. I have made it extendable for more weeks.
2. To make it scalable for new sports, I have introduced a new class called sport. Thereby, sports will have teams, teams will have depth charts and players.
3. New sports may have rules different to each other. Therefore, a factory pattern is used to differentiate the logic among them. Used an abstract class so it can have the common logic and can be overriden if needed.
4. Repository is setup as an in-memory singleton dictionary as there is no specific requirement mentioned for a data source. Anyhow, I have abstracted the repository and registered it as a keyed service. Therefore, if a new data source is introduced, it can be used without any hassle
5. Nothing much about the executable. Therefore, used a console application approach than a web api.
