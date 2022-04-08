Symbol link erstellen
sudo ln -s /etc/sites-available/CONFIG /etc/sites-enabled/config
sudo nginx -t
sudo services nginx restart
