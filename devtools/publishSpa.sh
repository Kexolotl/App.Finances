rm -R /home/pi/publishedSPAs/App.Finances
cd ../frontend
yarn install
npm run build
mv dist/ /home/pi/publishedSPAs/App.Finances
