CREATE USER 'istiyak'@'%' IDENTIFIED BY 'bottomhalf';
GRANT ALL PRIVILEGES ON *.* TO 'istiyak'@'%';
FLUSH PRIVILEGES;

Create database bh_test;