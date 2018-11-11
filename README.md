# Participants counter
This bot is helpful for counting participants of some meeting or event in group chats.
User need to send + if he/she is ready to participate, - if wan't.
Also you can use "+-" if you are not sure.
Some advanced usages like "+ 2 with me" or "+ John" are possible.
Bot will automatically display current counter after each "count" message.
Counter will be preserved for 3 days and after that previous value will be ignored.

# Commands
* **Count** ( /count or /c ) shows current count;
* **List** ( /list or /l ) shows full list of participants;

# Build and run

## Framework-dependent deployment

1.
* **Visual Studio** Publish ParticipantsCounter.App project via context menu and select "Framework-dependent deployment" in profile settings
* **Visual Studio Code** Publish ParticipantsCounter.App project via comand *dotnet publish* inside ParticipantsCounter.App folder

2. Go to directory Release\netcoreapp2.1\publish
3. Start application inside command line with command *dotnet ParticipantsCounter.App.dll*

## Self-contained deployment

1.
* **Visual Studio** Publish ParticipantsCounter.App project via context menu and select "Self-contained deployment" in profile settings
* **Visual Studio Code** Publish ParticipantsCounter.App project via comand (example for Windows 10 x64 platform) *dotnet publish -c Release -r win10-x64* inside ParticipantsCounter.App folder

2. Go to directory Release\netcoreapp2.1\publish
3. Start application using exe file ParticipantsCounter.App.exe

## Maintanance notes
* In the current implementation you need to restart application after several days to reload its state
* To stop application press Enter key
* Using several instances of bot for the same chats is not the case. Reuse application with another bot if you want to manage hosting. For that update bot token in the appsettings.json file
