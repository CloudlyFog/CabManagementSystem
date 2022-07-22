


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

## API
### AppContext
There are 3 classes context:

 1. **ApplicationContext** - is responsible for handling of user's resources.
 2. **OrderContext** - is responsible for handling of order's resources.
 3. **TaxiContext** - is responsible for handling of taxi's resources.


#### API for ApplicatonContext
 
**Methods:**
 1. `public  List<TaxiModel> DeserializeTaxiData(string path)` - method for deserializing of taxi's data from json file which of file path specified in parameters.
 2. `public  static  List<object> GetTaxiPropList(string taxiProp)` - method for getting list of definite taxi's property (ID,  TaxiClass etc.).
 3. `HashPassword(string password)` - is hashing got in parameters string (password).

**Properties:**

 1. `public  DbSet<UserModel> Users { get; set; }` - an instance of the table `Users` in database.
 2. `public  DbSet<AdminHandlingModel> AdminHandling { get; set; }` - property for handling operatoins in admin panel.

#### API OrderContext
**Methods:**

 1. `public  bool  AlreadyOrder(Guid userID)` - returns true or false depending on so, user with definite ID already order taxi or not.

**Properties:**

 1. `public  DbSet<OrderModel> Orders { get; set; }` - an instance of the table `Orders` in database.
 2. `public  DbSet<DriverModel> Drivers { get; set; }` - an instance of the table `Drivers` in database.
 3. `public  DbSet<TaxiModel> Taxi { get; set; }` - an instance of the table `Taxi` in database.

#### API TaxiContext
**Methods:**

 1. `protected  ExceptionModel  AddBindTaxiDriver(BindTaxiDriver bindTaxiDriver)` - creates a bind between driver and his taxi car and adds it to database.
 2. `protected  ExceptionModel  DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)` - deletes got bind between driver and his taxi car from database.

**Properties:**

 1. `public  DbSet<TaxiModel> Taxi { get; set; }` - an instance of the table `Taxi` in database.
 2. `public  DbSet<BindTaxiDriver> BindTaxiDriver { get; set; }` - an instance of the table `BindTaxiDriver` in database.


### Services
Services are dividing on 2 sub-folders:

 1. **Interfaces**
 2. **Repositories**

### Interfaces
Here located interfaces which describes behavior of inherited repo-classes.
 1. Interface `IRepository<T>` - parent interface from which will inherite other interfaces and repo-classes. Describes main logic and structure of the project.<br>**Methods**:  <br>- `ExceptionModel  Create(T item);` - implements creating item and adding it in database. <br>- `ExceptionModel  Update(T item);` -implements updating item in database.<br>- `ExceptionModel  Delete(T item);` - implements deleting item from database.<br>- `IEnumerable<T> Get();` - implements getting a sequence of the objects from database.<br>- `T Get(Guid id);` - implements getting an object from database with definite ID.<br> - `T  Get(Expression<Func<T, bool>> predicate);` - implements getting an object from database with func-condition.<br>- `bool  Exist(Guid id);` - implements checking exist object with definite ID in database or not.<br>- `bool  Exist(Expression<Func<T, bool>> predicate);` - implements checking exist object with func-condition in database.<br><br>
 3.  Interface `IUserRepository<T> : IRepository<T>` - describes implementations for handling user's resources.<br>**Methods:**<br> -`string  HashPassword(string password);` - implements hashing of user's password.<br> <br>
 4.  Interface `IOrderRepository<T> : IRepository<T>` - describes implementation for orders.<br>**Methods:**<br>- `bool  AlreadyOrder(Guid id);` - implements checking user already order taxi or not.<br> <br>
 5.  Interface `IDriverRepository<T>` - describes implementations for handling resources of drivers. Needs for implementation `DriverRepository<T>`.<br>**Methods:**<br>- `IEnumerable<T> Get();` - implements getting a sequence of the objects from database.<br>- `T  Get(Guid id);` - implements getting an object from database with definite ID.<br>- `T  Get(Expression<Func<T, bool>> predicate);` - implements getting an object from database with func-condition.<br> <br>
 6.  Interface `ITaxiRepository<T> : IRepository<T>` - describes implementations for handling resources of taxis. Being that `ITaxiRepository<T>` is inherited from `IRepository<T>`, `ITaxiRepository<T>` not needed in own implemantation and internal methods thats why it contains only method<br> `void  ChangeTracker();` - implements method `ChangeTracker(...)` from class `DbContext`.
 

### When can cause exceptions and errors?

 1. You didn't specify your database in connection string.
 2. You change workable structure of project. Something like you change content of context class, for example, you override method `OnConfiguring()` with other connection string.
 **Example**: 
 Instead of this 

````
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(queryConnection);
        }
````
 you'll write something like this
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
**CabManagementSystem** - in first time is project for learning framework and adjacent technologies.
If you have some suggestions or critics about this project you may either write me to socials or create pull request. <Br>
**Sincerely, CloudlyFog.**
