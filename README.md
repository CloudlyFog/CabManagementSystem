

# Project CabManagementSystem

# Documentation for API
## Structure of project

 1. Folder **AppContext** is folder where contain context classes for interaction with database without buisness logic.
 2. Folder **Controllers** contains classes with controllers including api-controllers.
 3. Folder **Data** - folder with MS SQL project and sql files for creating template of database.
 4. Folder **Models** contains in self all models which are using in project.
 5. Folder **Services** contains 2 sub folders: **Interfaces** and **Repositories**.
 6. Folder **Interfaces** contains interfaces which describes structure of inherited classes.
 7. Folder **Repositories** is folder with all buisness logic of project. Only there simple developer has access.
 8. Folder **Views** - folder with views of project.

## How to interact with database?
Developer can interact with database using variables queryConnection in repo-classes. These variables propose connection to database. Dev also can use public methods of repo-classes.
#### Remember!
If you'll not change connection string to database in repo-classes program may don't work correctly.
You can catch exception like "There isn't database which has been specified." because databases which was used in developing project may doesn't exist on your machine.

### When can cause exceptions and errors?

 1. You didn't specify your database in connection string.
 2. You change workable structure of project. Something like you change content of context class, for example, you override method OnConfiguring() with other connection string.
 **Example**: 
 Instead of this 

````
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(queryConnection);
        }
````
 you write something like this
 ````
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer();
        }
````
 In that case connection string won't specified and you'll get exception.
 
## Warning!
Implementation own interfaces can cause to **errors** and **crashing** of program! 
Changing workable structure of project also can cause to **errors** and **crashing** of program.
You change and complament buisness logic on your own discretion!


## Conclusion
CabManagementSystem - in first time is project for learning framework and adjacent technologies.
If you have some suggestions or critics about this project you may either write me to socials or create pull request. 
Sincerelly, CloudlyFog.
