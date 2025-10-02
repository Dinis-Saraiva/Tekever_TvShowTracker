@echo off
setlocal

:: ==============================
:: Config
:: ==============================
set "BASE_URL=https://localhost:7211"
set "COOKIE_FILE=cookies.txt"

:: ==============================
:: 1. Register a new user
:: ==============================
echo === 1. Register a new user ===
curl -k -X POST "%BASE_URL%/api/auth/register" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"alice\",\"email\":\"alice@test.com\",\"password\":\"Password123!\"}"
echo.

:: ==============================
:: 2. Login with the user
:: ==============================
echo === 2. Login with the user ===
curl -k -X POST "%BASE_URL%/api/auth/login" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"alice\",\"password\":\"Password123!\"}" ^
  -c "%COOKIE_FILE%"
echo.

:: ==============================
:: 3. Get current user
:: ==============================
echo === 3. Get current user (with cookies) ===
curl -k "%BASE_URL%/api/auth/current-user" -b "%COOKIE_FILE%"
echo.

:: ==============================
:: 4. Logout
:: ==============================
echo === 4. Logout ===
curl -k -X POST "%BASE_URL%/api/auth/logout" -b "%COOKIE_FILE%" -c "%COOKIE_FILE%"
echo.

:: ==============================
:: 5. Check current user after logout
:: ==============================
echo === 5. Check current user after logout (should be 401) ===
curl -k -i "%BASE_URL%/api/auth/current-user" -b "%COOKIE_FILE%"
echo.

:: ==============================
:: 6. Delete the user
:: ==============================
echo === 6. Login again for deletion ===
curl -k -X POST "%BASE_URL%/api/auth/login" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"alice\",\"password\":\"Password123!\"}" ^
  -c "%COOKIE_FILE%"
echo.

echo === 7. Delete user ===
curl -k -X DELETE "%BASE_URL%/api/auth/delete" -b "%COOKIE_FILE%"
echo.

echo ===8. Check user deletion===
curl -k -X POST "%BASE_URL%/api/auth/login" ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"alice\",\"password\":\"Password123!\"}" ^
  -c "%COOKIE_FILE%"
echo.

pause
endlocal
