@echo off
SETLOCAL

:: ----------------------------
:: Configuration
:: ----------------------------
SET "BASE_URL=https://localhost:7211"
SET "COOKIE_FILE=cookies.txt"
SET "TVSHOW_ID=1"
SET "PDF_FILE=TvShow_%TVSHOW_ID%.pdf"

:: ----------------------------
:: Step 1: Fetch TV show by ID
:: ----------------------------
echo === 3. Fetch TV show with ID %TVSHOW_ID% ===
curl -k -i "%BASE_URL%/api/tvshow/GetTvShowByID/%TVSHOW_ID%" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 2: Download TV show PDF
:: ----------------------------
echo === 4. Download PDF for TV show ID %TVSHOW_ID% ===
curl -k -o "%PDF_FILE%" "%BASE_URL%/api/tvshow/%TVSHOW_ID%/export-pdf" -b "%COOKIE_FILE%"
echo PDF saved as %PDF_FILE%
echo.

pause
ENDLOCAL
