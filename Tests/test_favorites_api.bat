@echo off
SETLOCAL

:: ----------------------------
:: Configuration
:: ----------------------------
SET "BASE_URL=https://localhost:7211"
SET "COOKIE_FILE=cookies.txt"


:: ==============================
:: Step 0: Register a new user
:: ==============================
echo === 1. Register a new user ===
curl -k -X POST "%BASE_URL%/api/auth/register" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"alice\",\"email\":\"alice@test.com\",\"password\":\"Password123!\"}"
echo.


:: ----------------------------
:: Step 1: Login with user
:: ----------------------------
echo === 1. Login ===
curl -k -i -X POST "%BASE_URL%/api/auth/login" -H "Content-Type: application/json" -d "{\"username\":\"alice\",\"password\":\"Password123!\"}" -c "%COOKIE_FILE%" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 2: List favorite TV shows (should be empty)
:: ----------------------------
echo === 2. Get favorite TV shows (empty) ===
curl -k -i "%BASE_URL%/api/favorites" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 3: Add a favorite TV show
:: ----------------------------
echo === 3. Add favorite TV show with ID 1 ===
curl -k -i -X POST "%BASE_URL%/api/favorites?tvShowIds=1" -H "Content-Type: application/json" -d "{}" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 4: List favorite TV shows again
:: ----------------------------
echo === 4. Get favorite TV shows (should contain ID 1) ===
curl -k -i "%BASE_URL%/api/favorites" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 5: Remove favorite TV show
:: ----------------------------
echo === 5. Remove favorite TV show with ID 1 ===
curl -k -i -X DELETE "%BASE_URL%/api/favorites?tvShowId=1" -b "%COOKIE_FILE%"
echo.

:: ----------------------------
:: Step 6: List favorite TV shows (should be empty again)
:: ----------------------------
echo === 6. Get favorite TV shows (empty again) ===
curl -k -i "%BASE_URL%/api/favorites" -b "%COOKIE_FILE%"
echo.

pause
ENDLOCAL
