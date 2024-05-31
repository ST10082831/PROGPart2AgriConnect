**README.md**

**AgriEnergyConnect**
=====================

**Overview**
----------

AgriEnergyConnect is a web-based application that connects farmers with customers, allowing them to buy and sell agricultural products. The application uses Entity Framework Core to interact with a SQL Server database.

**Features**
------------

* Farmer registration and login
* Product listing and purchase functionality
* Order tracking and management
* Customer registration and login
* Search and filter functionality for products and farmers

**Getting Started**
-------------------

1. Clone the repository: `git clone https://github.com/ST10082831/PROGPart2AgriConnect

**Database Setup**
------------------

To set up the database, follow these steps:

1. Open Packet Manager
2. Run the migration script: `Update-Database`
3.Open SQL Server Explorer , Under "SQL Server" Right Click on (localdb) press rename then copy that path.
4.Configure the connection string in the `appsettings.json` file by highlighting (localdb)\\MSSQLLocalDB in the connection string and then past the path from step 3.

**Contributing**
--------------

If you'd like to contribute to AgriEnergyConnect, please fork the repository and submit a pull request.

**License**
---------

AgriEnergyConnect is licensed under the MIT License.

**Acknowledgments**
-----------------

* [Colby] for creating this application
* Entity Framework Core for providing a robust ORM solution