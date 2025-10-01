@echo off
echo Setting up TV Show Tracker API project...

REM ---- Backend ----
cd backend\TvShowTracker.Api

echo Restoring .NET dependencies...
dotnet restore

echo Building backend project...
dotnet build

REM Database assumed to be already set up
echo Database assumed to be in correct location.

REM ---- Frontend ----
cd ..\..\tv-show-tracker

echo Installing frontend dependencies...
npm install

echo Building frontend...
npm run build

echo Setup complete!
echo Starting backend and frontend...

