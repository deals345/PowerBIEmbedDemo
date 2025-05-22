@echo off
setlocal

:: Set the Node.js path
set NODE_PATH=C:\Program Files\nodejs
set PATH=%NODE_PATH%;%PATH%

:: Verify Node.js and npm
node --version
npm --version

:: Create Angular app
echo Creating Angular application...
npx @angular/cli@latest new frontend --routing --style=scss --skip-git --minimal --skip-install

:: Navigate to frontend and install dependencies
cd frontend
npm install

endlocal
