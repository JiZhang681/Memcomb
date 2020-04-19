Project Name:
Memcomb


Link To GitHub Repository: https://github.com/LetsPlayer/Memcomb


Description:
Memcomb is a social media platform designed to connect users through their memories, and thereby forming a 360 degree view of the memory. Users will post “Memories”, or events in their lives, such as a family trip, in the form of images with dates, locations, and captions. Memcomb will then search for other users who have posted similar Memories. Those users will then be connected to form conversations and share their perspectives of their combined Memories. Thus creating new friendships and more complete memories. 


Getting Started


Prerequisite:
* Visual Studio 2019 to open and run the project
* Microsoft SQL Server 2019 to run the database
* Microsoft SQL Server Manager to manage the database
* Memcomb GitHub repository
* Azure devops 
* Two google cloud Virtual Machines running Windows Server 2019 Datacenter(one for web server and one for database server)


Deployment Process of System: 


Client:
To start, download the master branch of the github repository and save it with an easily accessible folder within your local system. Run Visual Studio 2019, open a new project and navigate to Memcomb.sln within your new folder. Build the project after successfully opening it within Visual Studio, and then press f5 to debug, or ctrl-f5, to start without debugging, to run the application. You should then seen a local instance of the login page.


Next, open Microsoft SQL Server Manager. For the server connection, type localhost and click connect. Then right click on Databases in the Object Explorer and click New Database. Enter memcombdb as the database name and click ok. Save and close out of Microsoft SQL Manager. Open your Windows Task Manager, go to the Services tab, and stop the service called MSSQLSERVER. Find the folder in the project folder titled Database. Inside is a zipped folder that contains two files, memcombdb and memcombd_log. Move these files into C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA. Finally, start the MSSQLSERVER service back up and your database is all set up.




Server:
Memcomb is deployed on an IIS web server using Azure DevOps. In the case of Memcomb we used two Google Cloud Platform Virtual Machines running Windows Server 2019 Datacenter to host the web server and database server for the project. Azure devops was used to create a pipeline that was deployed to the IIS web server. The virtual machine used as an IIS server must be added to the Azure Devops deployment group. Once the pipeline is released on the IIS server the website should be available at the external IP Address of IIS Server. To set up the database server Microsoft SQL Server and Microsoft SQL Management Studio must be installed on the second Virtual Machine. The database file memcombdb.mdf and memcomb_log.ldf need to be copied into the location of where the Microsoft SQL Server is installed. To add the Database into the server, attach it within Microsoft SQL Server Management Studio. A remote SQL user must then be created to allow for remote connections. If all the ports on the server are open the database server should then be able to accept connections.






How to connect the database to the visual studio project:


* Right click the Models Folder and select Add > New Item
* Under Data select ADO.NET Entity Data Model
* Name it memcombDB, click add
* Select EF Designer from database, click next
* Select New Connection and within Server Name enter in the name of your database server
   * You can find the server name within MSSQL by selecting Connect Object Explorer
* Make sure Select or enter a database name is selected, and choose your database from the drop down list
* Under Save connection settings in Web.Config as, memcombdbEntities, click next
* Within the check box group, select the tables check box, to select all tables
   * To select specific tables, click the drop down symbol, click it again under dbo, and select the tables you want to add
* Double check that both Pluralize or singular generated object names and Include foreign key columns in Model are checked
* Name the model, under Model Namespace, memcombdb
* Select Finish


Built With:
* ASP.Net MVC 5
* Entity Framework


README Authors
Noah Bradley 
Kyle King
Bryce Waters