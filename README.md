# Device Data Processor

This application is designed to be able to receive json files that are of various schemas and consolidate them into a single file. These files contain temperature and humidity data.

## Description

The application can be interacted with through both an API and a console app. Example json files are provided in the TestData folder at the root level of the solution.

## Getting Started

### Dependencies

* .NET 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Git

### Installing

* Pull down the project to your desired directory
* Open command line or terminal and navigate to where you wish to install the project
* Type in the following command
```
git clone https://github.com/ascherer1993/DeviceDataProcessor.git
```
* The project should clone into the current directory

## Executing The Application
In most IDEs, you should be able to open the solution, and run the api or console by selecting it as the startup project. Keep in mind that if you run the console app, you will also have to specify arguments that are paths to json files. If you run the API, you will need to follow a lot of the steps specified below in the 'Running the web API in terminal/commandline' section.

### Running the console app in terminal/commandline
* Navigate to the directory containing the .sln file
* Run the following command
```
dotnet build
```
* Navigate to the folder containing the console application
  * From the folder containing the sln file, navigate to the folder containing the console app build
{repositorylocation}/DeviceDataProcessor/DeviceDataProcessor.ConsoleApp/bin/Debug/net6.0
  * To do so in one command from the directory containing the .sln, type in the following command
```
cd ./DeviceDataProcessor.ConsoleApp/bin/Debug/net6.0
```
* Run the application using the following command where {jsonFile1Path} and {jsonFile2Path} are the paths to the files you wish to merge
```
dotnet DeviceDataProcessor.ConsoleApp.dll "{jsonFile1Path}" "{jsonFile2Path}"    

example with the test files if added to the build folder (where the console app dll is):
dotnet DeviceDataProcessor.ConsoleApp.dll DeviceDataFoo1.json DeviceDataFoo2.json    

Mac Example with absolute location
dotnet DeviceDataProcessor.ConsoleApp.dll "/Users/aaron/Documents/Development/Projects/DeviceDataProcessor/TestData/DeviceDataFoo1.json" "/Users/aaron/Documents/Development/Projects/DeviceDataProcessor/TestData/DeviceDataFoo2.json"   

Windows Example with absolute location
dotnet DeviceDataProcessor.ConsoleApp.dll "E:\Documents\Development\DeviceDataProcessor\TestData\DeviceDataFoo1.json" "E:\Documents\Development\DeviceDataProcessor\TestData\DeviceDataFoo2.json"
```
* The application should run. If it runs successfully, you should see a message with a path to where the merged file with the new format is located

### Running the web API in terminal/commandline
* Navigate to the directory containing the .sln file
* Run the following command
```
dotnet build
```
* Navigate to the folder containing the API application
    * From the folder containing the sln file, navigate to the folder containing the console app build
      {repositorylocation}/DeviceDataProcessor/DeviceDataProcessor.API/bin/Debug/net6.0
    * To do so in one command from the directory containing the .sln, type in the following command
```
cd ./DeviceDataProcessor.API/bin/Debug/net6.0
```
* Run the application using the following command:
```
dotnet DeviceDataProcessor.API.dll
```
* The application should now inform you that it is running on localhost
* Copy the https url and paste it in the browser of your choice. You will use this port number later to make a request
* You most likely will get some sort of warning telling you that the connection is not private. To continue you need to click to visit the site anyways
  * Note: You can append /swagger/index.html to the url to view the API Documentation (https://localhost:5001/swagger/index.html)
* To send the request:
  * Using an API tool like Postman (https://www.postman.com), make a post request to https://localhost:5001/api/v1/DeviceData/UploadJsonFiles
  * You will need to use a content-type header of 'multipart/form-data'
  * Select form data for the body format
  * Add a key named jsonFiles
  * For the value, you will need to select one or more json files that fit one of the accepted schemas
* The application should run. If it runs successfully, you should get a response with the merged objects with the new format
  * Note: A json file should also be created containing the list. It should be located in an output directory in the same location as the API build




## Running tests
* Navigate to the folder containing the Tests
  * From the folder containing the sln file, navigate to the folder containing the console app build
    {repositorylocation}/DeviceDataProcessor/DeviceDataProcessor.Tests
* Run the following command
```
dotnet test
```