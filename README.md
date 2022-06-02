### Easy_Save

Made with [![My Skills](https://skills.thijs.gg/icons?i=c#,.net)](https://skills.thijs.gg)

## Usage

The EasySave application gives the user the possibility to move a quantity of important files to a destination that the user will have to select. The user will be free to select the source directory where files, folders and subfolders are stored.

The user will also be able to choose two types of backup jobs (copying files from one directory to another). These two types of jobs are Full or Differential. In full mode, the application moves all the files in the source directory to the destination directory.

![easysave](https://user-images.githubusercontent.com/73825898/171683085-11285c71-c171-4fb9-abe3-41310b74be25.png)

In differential mode, the files in the source directory are compared with the files in the destination directory. If files are identical between the two directories, these files will not be moved.

A set of logs is used, like the logStatus allowing to know where is located the backup job in real time, then the logDaily allowing to know all the files that have been copied.

## Features

# Languages

The application has the option of changing the interface language on all possible views. The application is scalable, you just have to load a language file in JSON format with the right keys to add it to the language dropdown menu, it is easy to set up a new language within it, we currently have 3 languages: French, English, Romanian.
![unknown](https://user-images.githubusercontent.com/73825898/171683396-ca1cbebb-f66c-46c9-bca3-d2cda6144374.png)

# Save Work

The backup job is broken down into several parts, instructions and features to be as efficient as possible. As part of the specification, an unlimited number of backups were required which must be able to be saved and configured to run individually, and/or together.

The whole backup job is based on the above entities. From the main menu, we are offered an option "Backup job", when the user selects this option, he/she arrives on a view called ListJobView.

